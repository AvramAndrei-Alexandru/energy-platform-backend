using EnergyDataPlatform.src.Application.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyDataPlatform.src.Application.Services.Interfaces
{
    public interface IUserService
    {
        IEnumerable<IdentityError> Register(UserModel userModel);
        string Login(LoginUserModel userModel);
        List<DashboardUserModel> GetAllUsers();
        void UpdateUser(DashboardUserModel user);
        void DeleteUser(string id);
        bool ExistsUser(string userName);
    }
}
