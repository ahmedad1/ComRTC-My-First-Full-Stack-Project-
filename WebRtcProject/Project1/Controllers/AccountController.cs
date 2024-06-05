
using DataProject.CQRS.Commands;
using DataProject.CQRS.Queries;
using DataProject.Data.Dto;
using DataProject.Tokens;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SmallProject.EmailService;
using System.IdentityModel.Tokens.Jwt;


namespace SmallProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AccountController : ControllerBase
    {
        private IMediator mediator;
        private IOptions<MailSettings> mailSettings;
        private ISendService mailService;
        private ITokens refreshAllTokens;

        public AccountController(IMediator mediator, ISendService mailService, ITokens refreshAllTokens,IOptions<MailSettings>mailSettings)
        {
            this.mediator = mediator;
            this.mailService = mailService;
            this.refreshAllTokens = refreshAllTokens;
            this.mailSettings = mailSettings;
        }
        [HttpPost("SignUp")]
        public async Task<IActionResult> Register(SignUpDto signuUpDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!await ValidateReCaptcha(signuUpDto.Recaptcha))
            {
                return Forbid();
            }

            var result = await mediator.Send(new SignUp(signuUpDto));
            if (!result)
                return BadRequest("You entered invalid data");

            return Ok(result);

        }
        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var IsValidRecaptcha = await ValidateReCaptcha(loginDto.Recaptcha);
            if (!IsValidRecaptcha)
                return Forbid();
            var result = await mediator.Send(new Login(loginDto));
            if (!result.EmailConfirmed)
            {
                return Ok(new
                {
                    EmailConfirmed = false, Email = result.Email
                });
            }
            if (!result.Success)
                return BadRequest("invalid Operation");
            setCookie("jwt", result.JwtToken, DateTime.UtcNow.AddMinutes(60));
            setCookie("refreshToken", result.RefreshToken, DateTime.UtcNow.AddHours(6));
            return Ok(new { EmailConfirmed = true, Email = result.Email, FullName = result.FullName, Username = result.Username, Expiration = result.Expiration });
        }
        [HttpPost("Verify")]
        public async Task<IActionResult> VerifyEmail([FromBody] string email)
        {


            var rand = new Random();
            var verificationNum = rand.Next(1000, 9999999);
            var body = $"Dear {email} you have signed up on out website\n, and we have sent to you a verification code which is : <b>{verificationNum}</b> ";

            var success = await mediator.Send(new InsertVerifcationCode(email, verificationNum.ToString()));
            if (success == true)
            {
                await mailService.SendEmail(email, "Email Verification", body);
                var token=await mediator.Send(new AddIdentityToken(email));
                setCookie("identityToken", token, DateTime.UtcNow.AddMinutes(25));
            }

            return Ok(success);

        }
        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailReqDto confirmDto)
        {
            if(!Request.Cookies.TryGetValue("identityToken",out string? token))
                return Ok(new {EmailConfirmed=false});
            if(!await mediator.Send(new ValidateIdentityToken(confirmDto.Email,token)))
                return Ok(new { EmailConfirmed = false });
            var result = await mediator.Send(new ValidateCodeOfVerification(confirmDto.Email, confirmDto.Code));
            if (!result.EmailConfirmed)
            {
                return Ok(new
                {
                    EmailConfirmed = false
                });
            }
            setCookie("jwt", result.JwtToken, DateTime.UtcNow.AddMinutes(60));
            setCookie("refreshToken", result.RefreshToken, DateTime.UtcNow.AddHours(6));

            return Ok(new { EmailConfirmed = true, Email = result.Email, FullName = result.FullName, Username = result.Username, Expiration = result.Expiration });
        }
        private void setCookie(string key, string value, DateTime expires)
        {
            var cookieOptions = new CookieOptions();
            cookieOptions.Expires = expires.ToLocalTime();
            cookieOptions.HttpOnly = true;
            cookieOptions.SameSite = SameSiteMode.Strict;
            cookieOptions.Secure = true;


            Response.Cookies.Append(key, value, cookieOptions);

        }
        [HttpPost("UpdateToken")]

        public async Task<IActionResult> RefTokens()
        {
            if (Request.Cookies.TryGetValue("refreshToken", out string? val))
            {
                SecurityToken? jwt = null;
                //int expafter = 0;
                //bool jwtfound = false;
                if (await mediator.Send(new IsActiveRefreshToken(val)))
                {
                    if (Request.Cookies.TryGetValue("jwt", out string? val2))
                    {

                        jwt = new JwtSecurityTokenHandler().ReadToken(val2);
                        var date = jwt.ValidTo.ToLocalTime();

                        if (date - DateTime.Now > TimeSpan.FromSeconds(20))
                        {
                            return Ok(new { Expiration = jwt.ValidTo });
                        }


                        //jwtfound = true;
                    }



                } else return BadRequest("Invalid Token");

                var newTokens = await refreshAllTokens.UpdateTokens(val);
                if (newTokens == null)
                    return NotFound("Invalid Token");

                setCookie("jwt", newTokens.JwtToken, DateTime.Now.AddMinutes(60));
                setCookie("refreshToken", newTokens.RefreshToken, DateTime.Now.AddHours(6));

                return Ok(new { Expiration = newTokens.Expiration });
            }
            return BadRequest("Token is not found in cookies");
        }
        [HttpPost("SignOut")]
        public async Task<IActionResult> AddTokenInBlackList()
        {
            var Jwt = Request.Cookies["jwt"];
            if (Jwt == null)
                return BadRequest();
            var refToken = Request.Cookies["refreshToken"];
            if (refToken is null)
                return BadRequest();
             await mediator.Send(new AddJwtInBlackList(Jwt));
             await mediator.Send(new RevokeRefreshToken(refToken));
            
            setCookie("jwt", " ", DateTime.UtcNow.AddDays(-2));
            if (Request.Cookies.TryGetValue("refreshToken", out string? val))
            {
                setCookie("refreshToken", " ", DateTime.UtcNow.AddDays(-2));
            }

            return Ok();

        }
        [HttpPost("Load")]
        //endpoint for onload in js in index.html that check if the user logged in already and this endpoint check if the username and email is for the same user or no
        public async Task<IActionResult> CheckLoad(OnLoadCheckDto onloadcheckDto)
        {
            if (Request.Cookies.TryGetValue("refreshToken", out string? val))
            {
                return Ok(new { Matched = await mediator.Send(new CheckOnLoad(onloadcheckDto, val)) });

            }
            else
                return Ok(new { Matched = false });

        }
        [Authorize]
        [HttpPost("MakeNewRoom")]

        public IActionResult MakeNewRoom()
        {
            return Ok(new { roomName = Guid.NewGuid().ToString() });
        }
        [Authorize]
        [HttpPost("CheckRoom")]
        public async Task<IActionResult> CheckRoom([FromBody] string roomName)
        {
            bool res = await mediator.Send(new CheckGroupName(roomName));
            return res ? Ok() : NotFound();

        }
        [HttpPost("ContactUs")]
        public async Task<IActionResult> ContactUs([FromBody] ContactUsDto contactUs)
        {
            if(!ModelState.IsValid) { 
            return BadRequest(ModelState);
            }
            var IsValidRecaptcha = await ValidateReCaptcha(contactUs.Recaptcha);
            if (!IsValidRecaptcha)
                return Forbid();
            await mailService.SendEmail(mailSettings.Value.Email, $"Feedback ComRTC From {contactUs.Email}", contactUs.Message);
            return Ok(contactUs.Email);
        }
        private async Task<bool> ValidateReCaptcha(string captcha)
        {
            bool result=false;
            using (var httpClient=new HttpClient())
            {
                var secret = "6LfmLVYpAAAAAL0EeuM7bmSFFFWVNuieVbNIETKx";
                
                var url = $"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={captcha}";
     
                var response = await httpClient.PostAsync(url, null);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadFromJsonAsync<ResponseReCaptcha>();
                 
                    result = responseBody!.success;
                }
            }
            return result;
        }
    }
}
