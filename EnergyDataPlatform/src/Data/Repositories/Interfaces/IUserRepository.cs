using EnergyDataPlatform.src.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyDataPlatform.src.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        List<User> GetAllUsers();
        User GetUserByName(string userName);
        void UpdateUser(User user);
        void DeleteUser(string id);
        bool ExistsUser(string userName);
        
    }
}
