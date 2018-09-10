using System.Threading.Tasks;

namespace Email
{
    interface IEmailService
    {
        Task<bool> SendAsync(IEmailConfiguration config);
    }
}
