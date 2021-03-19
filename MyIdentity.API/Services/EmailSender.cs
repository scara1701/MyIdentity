using System.Threading.Tasks;

namespace MyIdentity.API.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            //implement logic for sending mails here
            return Task.CompletedTask;
        }
    }
}
