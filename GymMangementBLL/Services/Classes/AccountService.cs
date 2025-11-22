using GymMangementBLL.Services.Interfaces;
using GymMangementBLL.ViewModels;
using GymMangementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementBLL.Services.Classes
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountService(UserManager<ApplicationUser>userManager)
        {
            _userManager = userManager;
        }
        public ApplicationUser? ValidataUser(LoginViewModel loginViewModel)
        {
           var User = _userManager.FindByEmailAsync( loginViewModel.Email).Result;
            if (User != null)
            {
                var isPasswordValid = _userManager.CheckPasswordAsync(User, loginViewModel.Password).Result;
                if (isPasswordValid)
                {
                    return User;
                }
            }
            return null;

        }
    }
}
