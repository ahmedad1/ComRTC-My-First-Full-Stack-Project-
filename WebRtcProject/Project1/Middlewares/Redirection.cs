namespace Project1.Middlewares
{
    public class Redirection(RequestDelegate Next)
    {
        
        public async Task InvokeAsync(HttpContext context)
       {
            if (!context.Request.Path.Value!.StartsWith("/api") && (Path.HasExtension(context.Request.Path.Value) || context.Request.Path.Value == "/"))
            {
                var cookies = context.Request.Cookies;
                if (
                cookies.TryGetValue("refreshToken", out string? refToken) &&
                cookies.TryGetValue("username", out string? userName) &&
                cookies.TryGetValue("fullname", out string? fname) &&
                cookies.TryGetValue("email", out string? email) &&
                cookies.TryGetValue("emailconfirmed", out string? emailconfirmed) &&
                cookies.TryGetValue("expiration", out string? expiration)

                )
                {
                    if (context.Request.Path.Value.StartsWith("/index.html") || context.Request.Path.Value.StartsWith("/verification.html") || context.Request.Path.Value == "/")
                        context.Response.Redirect("/main.html");
                    else
                        await Next(context);
                }
                else if (context.Request.Path.Value.StartsWith("/verification.html"))
                {
                    if (!context.Request.Cookies.TryGetValue("email", out string? mail))
                        context.Response.Redirect("/index.html");
                    else
                        await Next(context);
                }
                else if (context.Request.Path.Value.StartsWith("/index.html")||(Path.HasExtension(context.Request.Path.Value)&&Path.GetExtension(context.Request.Path.Value)!=".html"))
                    await Next(context);
                else
                    context.Response.Redirect("/index.html");
                return;
            }
            await Next(context);
        }
    }
}
