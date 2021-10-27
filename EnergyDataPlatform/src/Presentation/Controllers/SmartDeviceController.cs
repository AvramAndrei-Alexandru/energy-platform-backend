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
    public class SmartDeviceController : ControllerBase
    {
        private readonly ISmartDeviceService _smartDeviceService;

        public SmartDeviceController(ISmartDeviceService smartDeviceService)
        {
            _smartDeviceService = smartDeviceService;
        }

        [HttpPost("Device")]
        [Authorize(Roles = "Admin")]
        public EmptyResult AddDevice([FromBody] SmartDeviceDashboardModel smartDevice)
        {
            _smartDeviceService.AddDevice(smartDevice);
            return new EmptyResult();
        }

        [HttpPut("Device")]
        [Authorize(Roles = "Admin")]
        public EmptyResult UpdateDevice([FromBody] SmartDeviceDashboardModel smartDevice)
        {
            _smartDeviceService.UpdateDevice(smartDevice);
            return new EmptyResult();
        }

        [HttpGet("Devices/{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<List<SmartDeviceDashboardModel>> GetAllDevicesForUser([FromRoute] string id)
        {
            return Ok(_smartDeviceService.GetAllDevicesForUser(id));
        }

        [HttpGet("Devices")]
        [Authorize(Roles = "Client")]
        public ActionResult<List<SmartDeviceDashboardModel>> GetAllDevicesForCurrentUser()
        {
            return Ok(_smartDeviceService.GetAllSmartDevicesForCurrentUser());
        }

        [HttpDelete("Device/{id}")]
        [Authorize(Roles = "Admin")]
        public EmptyResult DeleteDevice([FromRoute] Guid id)
        {
            _smartDeviceService.RemoveDevice(id);
            return new EmptyResult();
        }

    }
}