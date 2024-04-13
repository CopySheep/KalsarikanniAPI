using HotelFuen31.APIs.Services.RenYu;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelFuen31.APIs.Controllers.RenYu
{
    

    [Route("api/[action]")]
    [ApiController]
    public class SendEmailController : ControllerBase
    {
        private readonly SendEmailService _service;

        public SendEmailController(SendEmailService service)
        {
            _service = service;
        }

        [HttpPost]
        public void SendEmail(string subject, string title, string content, string toEmail)
        {
            _service.SendEmail(subject,title,content,toEmail);
        }

        [HttpPost]
        public void SendValidationEmail(string toEmail,string url)
        {
            _service.SendValidationEmail(toEmail, url);
        }
        [HttpPost]
        public void SendResetPwdEmail(string toEmail, string url)
        {
            _service.SendResetPwdEmail(toEmail, url);
        }
    }

}
