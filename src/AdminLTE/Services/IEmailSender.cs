using AdminLTE.Models;
using System.Threading.Tasks;

namespace AdminLTE.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
