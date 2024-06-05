namespace SmallProject.EmailService
{
    public interface ISendService
    {
       public  Task SendEmail(string to, string subject, string body, IList<IFormFile>? formFiles=null);
    }
}
