﻿using Identity.BusinessLogic.Models;
using Identity.BusinessLogic.Models.UserService;

namespace Identity.BusinessLogic.Services
{
    public interface ISignInService
    {
        Task<Tokens> SignInAsync(CheckPasswordRequest request);
        Task<Tokens> SignInWithRefreshAsync(Tokens tokens);
    }
}
