using EnergyDataPlatform.src.Application.Models;
using EnergyDataPlatform.src.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EnergyDataPlatform.src.Data.Mesaging
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MessageHub : Hub
    {
        private ISensorMeasurementService _measurementService;

        public MessageHub(ISensorMeasurementService measurementService)
        {
            _measurementService = measurementService;
        }

        public List<SensorMeasurementDashboardModel> GetMeasurements(GetMeasurementsForDeviceInIntervalModel model)
        {
            return _measurementService.GetDeviceHourlyMeasurementsInGivenInterval(model.DeviceID, model.StartDate, model.EndDate);
        }

        public List<SensorMeasurementDashboardModel> GetWeekBaseline(GetMeasurementsForDeviceInIntervalModel model)
        {
            return _measurementService.GetWeekBaseline(model.DeviceID, model.StartDate, model.EndDate);
        }

        public TimeToStartMeasurementModel GetBestTimeToStartAndMeasurements(int duration, GetMeasurementsForDeviceInIntervalModel model)
        {
            return _measurementService.GetBestTimeToStartAndMeasurements(duration, model.DeviceID, model.StartDate, model.EndDate);
        }
    }
}
