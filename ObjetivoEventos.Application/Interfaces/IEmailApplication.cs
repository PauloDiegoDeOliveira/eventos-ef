using ObjetivoEventos.Application.Dtos.SMTP;
using System.Threading.Tasks;

namespace ObjetivoEventos.Application.Interfaces
{
    public interface IEmailApplication
    {
        public Task SendEmail(UserEmailOptions userEmailOptions, string emailTemplate);
    }
}