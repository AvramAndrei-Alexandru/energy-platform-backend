using EnergyDataPlatform.src.Application.Services.Interfaces;
using EnergyDataPlatform.src.Data.Mesaging;
using EnergyDataPlatform.src.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyDataPlatform.src.Application.Services.Implementation
{
    public class MessagingService : IMessagingService
    {
        private readonly IHubContext<MessageHub> _hubContext;
        private readonly ISmartDeviceRepository _smartDeviceRepository;

        public MessagingService(IHubContext<MessageHub> hubContext, ISmartDeviceRepository smartDeviceRepository)
        {
            _hubContext = hubContext;
            _smartDeviceRepository = smartDeviceRepository;
        }

        public async void SendMessageToAllClients(Message message, decimal powerPeak)
        {
            var device = _smartDeviceRepository.GetDeviceById(message.DeviceId);
            var powerPeakMessage = new PowerPeakMessage
            {
                UserName = device.User.UserName,
                DeviceName = device.Description,
                TimeStamp = message.TimeStamp,
                PowerPeak = powerPeak
            };
            var payload = new Notification { Message = powerPeakMessage};
            Notification[] notificationArray = new Notification[1];
            notificationArray[0] = payload;
            await _hubContext.Clients.All.SendCoreAsync("Send", notificationArray);
        }

        public async void SendUpdateMessage()
        {
            var payload = new Notification { Message = null };
            Notification[] notificationArray = new Notification[1];
            notificationArray[0] = payload;
            await _hubContext.Clients.All.SendCoreAsync("Update", notificationArray);
        }
    }
}
