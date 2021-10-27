using EnergyDataPlatform.src.Application.Enums;
using EnergyDataPlatform.src.Application.Models;
using EnergyDataPlatform.src.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyDataPlatform.src.Application.Mappers
{
    public static class UserMapper
    {
        public static User ToUserEntity(UserModel userModel)
        {
            if(userModel == null)
            {
                return null;
            }

            return new User
            {
                BirthDate = userModel.BirthDate,
                Address = userModel.Address,
                UserName = userModel.UserName
            };
        }

        public static DashboardUserModel ToDashboardUserModel(User user, IList<string> roles)
        {
            if(user == null)
            {
                return null;
            }

            return new DashboardUserModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Address = user.Address,
                BirthDate = user.BirthDate,
                Role = roles[0] == RoleEnum.Admin.ToString() ? RoleEnum.Admin : RoleEnum.Client
            };
        }

        public static User ToUserEntity(DashboardUserModel user)
        {
            if (user == null)
            {
                return null;
            }

            return new User
            {
                Id = user.Id,
                Address = user.Address,
                BirthDate = user.BirthDate
            };
        }
    }
}
