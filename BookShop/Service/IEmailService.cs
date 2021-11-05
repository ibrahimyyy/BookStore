using BookShop.Models;
using System.Threading.Tasks;

namespace BookShop.Service
{
    public interface IEmailService
    {
        Task SendTestEmails(UserEmailOptions userEmailOptions);
        Task SendEmailsConfirmation(UserEmailOptions userEmailOptions);
        Task SendEmailsForgetPassword(UserEmailOptions userEmailOptions);
    }
}