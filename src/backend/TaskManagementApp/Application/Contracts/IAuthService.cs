﻿using Application.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(RegisterCommand command);
        Task<AuthResult> LoginAsync(LoginCommand command);
    }
}
