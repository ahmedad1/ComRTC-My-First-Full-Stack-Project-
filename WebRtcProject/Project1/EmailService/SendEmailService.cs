using DataProject.Data.Dto;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace SmallProject.EmailService
{
    public class SendEmailService:ISendService
    {
        private readonly IOptions<MailSettings> mailSettings;

        public SendEmailService(IOptions<MailSettings> mailSettings)
        {
            this.mailSettings= mailSettings;
        }
        public async Task SendEmail(string to, string subject, string body, IList<IFormFile>? formFiles = null)
        {
            var message = new MimeMessage();
            message.Subject = subject;
            message.From.Add(new MailboxAddress(mailSettings.Value.Name, mailSettings.Value.Email));
            message.To.Add(MailboxAddress.Parse(to));
            var builder=new BodyBuilder();
            builder.HtmlBody = body;
            if(formFiles != null) 
            foreach (var file in formFiles)
            {
                if (file.Length == 0)
                    continue;
                    byte[] arr;
                    using (var stream = new MemoryStream())
                    {

                        await file.CopyToAsync(stream);
                        arr = stream.ToArray();
                        builder.Attachments.Add(file.FileName,arr,ContentType.Parse(file.ContentType));

                    }


            }
            message.Body = builder.ToMessageBody();
            using (var smtp=new SmtpClient())
            {
               await smtp.ConnectAsync(mailSettings.Value.Host, mailSettings.Value.Port, SecureSocketOptions.StartTls);
               await smtp.AuthenticateAsync(mailSettings.Value.Email, mailSettings.Value.Password);
               await smtp.SendAsync(message);

            }
        }
    }
}
