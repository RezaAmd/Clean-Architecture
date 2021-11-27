using Application.Models;
using Domain.Enums;
using System.Threading.Tasks;

namespace Infrastructure.Common.Interfaces
{
    public interface IEmailService
    {
        Task<Result> SendAsync(string reciever, string subject, string body, FromEmail from = FromEmail.Noreplay, string recieverName = default);
    }
}