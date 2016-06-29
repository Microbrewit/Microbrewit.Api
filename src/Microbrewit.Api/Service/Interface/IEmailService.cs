using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microbrewit.Api.Service.Interface
{
    public interface IEmailService
    {
        Task SendResetPasswordMailAsync(string email,string token);
    }
}
