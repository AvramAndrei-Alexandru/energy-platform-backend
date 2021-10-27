using EnergyDataPlatform.src.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyDataPlatform.src.Application.Models
{
    public class UserModel
    {
        public string UserName { get; set; }
        public string Address { get; set; }
        public DateTime BirthDate { get; set; }
        public RoleEnum Role { get; set; }
        public string Password { get; set; }
    }

    public class LoginUserModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class DashboardUserModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public DateTime BirthDate { get; set; }
        public RoleEnum Role { get; set; }
    }
}
