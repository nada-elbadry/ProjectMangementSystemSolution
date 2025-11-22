using GymMangementBLL.ViewModels;
using GymMangementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementBLL.Services.Interfaces
{
    public interface IAccountService
    {
        ApplicationUser? ValidataUser(LoginViewModel loginViewModel);
    }
}
