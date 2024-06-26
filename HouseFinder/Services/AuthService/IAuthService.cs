﻿using HouseFinderBackEnd.Data.Models.UserModels;

namespace HouseFinderBackEnd.Services.AuthService
{
    public interface IAuthService
    {
        public Task<string> Register(RegisterModel model);
        public Task<string> Login(LoginModel model);
    }
}
