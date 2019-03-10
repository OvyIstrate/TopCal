using System.Threading.Tasks;

namespace TopCalAPI.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendMailAsync(EmailInfo model);
    }
}
