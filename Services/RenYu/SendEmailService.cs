using HotelFuen31.APIs.Uitilities;
using System.Net;
using System.Net.Mail;
using System.Security.Policy;
using System.Text;


namespace HotelFuen31.APIs.Services.RenYu
{
    public class SendEmailService
    {
        public void SendEmail(string subject, string title, string content,string toEmail)
        {
            SmtpEmail(subject,toEmail,() => EmailTemplate.General(title, content));
        }
        public void SendValidationEmail(string toEmail, string url, string subject = "Kalsari Hotel 會員認證信")
        {
            SmtpEmail(subject,toEmail,() => EmailTemplate.Validation(url));
        }
        public void SendResetPwdEmail(string toEmail, string url, string subject = "Kalsari Hotel 會員密碼重置")
        {
            SmtpEmail(subject, toEmail, () => EmailTemplate.ResetPwd(url));
        }
        private void SmtpEmail(string subject ,string toEmail ,Func<string> template)
        {
            string sender = "kalsarihotel@gmail.com";
            string appPassword = "tuastatfwzzxbhmg";

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(sender, appPassword),
                EnableSsl = true
            };

            MailMessage message = new MailMessage()
            {
                From = new MailAddress(sender),
                Subject = subject,
                Body = template(),
                IsBodyHtml = true,
                BodyEncoding = Encoding.UTF8,
            };

            message.To.Add(toEmail);

            smtpClient.Send(message);
        }
    }
}
