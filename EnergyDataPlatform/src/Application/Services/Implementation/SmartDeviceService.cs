using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnergyDataPlatform.src.Application.Mappers;
using EnergyDataPlatform.src.Application.Models;
using EnergyDataPlatform.src.Application.Services.Interfaces;
using EnergyDataPlatform.src.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace EnergyDataPlatform.src.Application.Services.Implementation
{
    public class SmartDeviceService : ISmartDeviceService
    {
        private readonly ISmartDeviceRepository _smartDeviceRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;

        public SmartDeviceService(ISmartDeviceRepository smartDeviceRepository, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _smartDeviceRepository = smartDeviceRepository;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        public void AddDevice(SmartDeviceDashboardModel smartDevice)
        {
            _smartDeviceRepository.AddDevice(SmartDeviceMapper.ToSmartDeviceEntity(smartDevice));
        }

        public List<SmartDeviceDashboardModel> GetAllDevicesForUser(string userId)
        {
            return _smartDeviceRepository.GetAllDevicesForUser(userId).Select(d => SmartDeviceMapper.ToSmartDeviceDashboardModel(d)).ToList();
        }

        public List<SmartDeviceDashboardModel> GetAllSmartDevicesForCurrentUser()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var dbUser = _userRepository.GetUserByName(user.Identity.Name);
            return _smartDeviceRepository.GetAllDevicesForUser(dbUser.Id).Select(d => SmartDeviceMapper.ToSmartDeviceDashboardModel(d)).ToList();
        }

        public void RemoveDevice(Guid id)
        {
            _smartDeviceRepository.RemoveDevice(_smartDeviceRepository.GetDeviceById(id));
        }

        public void UpdateDevice(SmartDeviceDashboardModel smartDevice)
        {
            var test = "";
            _smartDeviceRepository.UpdateDevice(SmartDeviceMapper.ToSmartDeviceEntity(smartDevice));
        } 
    } 
}
