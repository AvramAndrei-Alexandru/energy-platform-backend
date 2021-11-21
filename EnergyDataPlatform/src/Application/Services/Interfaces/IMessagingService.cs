using EnergyDataPlatform.src.Data.Mesaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyDataPlatform.src.Application.Services.Interfaces
{
    public interface IMessagingService
    {
        public void SendMessageToAllClients(Message message, decimal powerPeak);
        public void SendUpdateMessage();
    }
}
