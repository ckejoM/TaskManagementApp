using Application.Common;
using Application.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IAuthService
    {
        Task<Result<AuthResult>> RegisterAsync(RegisterCommand command);
        Task<Result<AuthResult>> LoginAsync(LoginCommand command);
    }
}
