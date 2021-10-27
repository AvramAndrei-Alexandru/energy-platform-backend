using EnergyDataPlatform.src.Data.Entities;
using EnergyDataPlatform.src.Data.Persistance;
using EnergyDataPlatform.src.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyDataPlatform.src.Data.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _context;

        public UserRepository(DatabaseContext context)
        {
            _context = context;
        }

        public void DeleteUser(string id)
        {
            var user = _context.Users.Include(u => u.SmartDevices).ThenInclude(u => u.SensorMeasurements).FirstOrDefault(u => u.Id == id);
            if(user != null)
            {
                user.SmartDevices?.ForEach(s =>
                {
                    _context.SensorMeasurements.RemoveRange(s.SensorMeasurements);
                });
                _context.SaveChanges();
                _context.SmartDevices.RemoveRange(user.SmartDevices);
            }
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public bool ExistsUser(string userName)
        {
            return _context.Users.FirstOrDefault(u => u.NormalizedUserName == userName.ToUpper()) != null;
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.OrderBy(u => u.UserName).Include(u => u.SmartDevices).ThenInclude(s => s.SensorMeasurements).ToList();
        }

        public User GetUserByName(string userName)
        {
            var normalizedUserName = userName.ToUpper();
            return _context.Users.Include(u => u.SmartDevices).ThenInclude(s => s.SensorMeasurements).FirstOrDefault(u => u.NormalizedUserName == normalizedUserName);
        }

        public void UpdateUser(User user)
        {
            var dbUser = _context.Users.FirstOrDefault(u => u.Id == user.Id);
            if(dbUser != null)
            {
                dbUser.BirthDate = user.BirthDate;
                dbUser.Address = user.Address;
            }
            _context.SaveChanges();
        }
    }
}
