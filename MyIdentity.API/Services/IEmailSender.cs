using System.Threading.Tasks;

namespace MyIdentity.API.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
