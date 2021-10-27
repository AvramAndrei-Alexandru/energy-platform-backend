using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnergyDataPlatform.src.Application.Models;
using EnergyDataPlatform.src.Application.Services.Interfaces;

namespace EnergyDataPlatform.src.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SensorMeasurementController : ControllerBase
    {
        private readonly ISensorMeasurementService _sensorMeasurementService;

        public SensorMeasurementController(ISensorMeasurementService sensorMeasurementService)
        {
            _sensorMeasurementService = sensorMeasurementService;
        }

        [HttpGet("SensorMeasurements/{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<List<SensorMeasurementDashboardModel>> GetAllMeasurementsForDevice([FromRoute] Guid id)
        {
            return Ok(_sensorMeasurementService.GetAllMeasurementsForDevice(id));
        }

        [HttpPost("SensorMeasurements/Day")]
        [Authorize]
        public ActionResult<List<SensorMeasurementDashboardModel>> GetAllMeasurementsForDeviceForGivenDay([FromBody] GetMeasurementModel model)
        {
            return Ok(_sensorMeasurementService.GetDeviceMeasurementsForGivenDay(model.DeviceId, model.StartDate, model.EndDate));
        }

        [HttpPost("SensorMeasurement")]
        [Authorize(Roles = "Admin")]
        public EmptyResult AddSensorMeasurement([FromBody] SensorMeasurementDashboardModel sensorMeasurementDashboard)
        {
            _sensorMeasurementService.AddSensorMeasurement(sensorMeasurementDashboard);
            return new EmptyResult();
        }
    }
}
