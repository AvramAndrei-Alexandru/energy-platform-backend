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
using Microsoft.Net.Http.Headers;

namespace EnergyDataPlatform.src.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        public readonly IUserService _userService;

        public UserController(IUserService userService): base()
        {
            _userService = userService;
        }

        [HttpPost("Register")]
        [Authorize(Roles = "Admin")]
        public IActionResult RegisterUser([FromBody] UserModel user)
        {
            var result = _userService.Register(user);
            if (result == null)
            {
                return Ok();
            }
            return new BadRequestObjectResult(result.ToList());
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginUserModel user)
        {
            var token = _userService.Login(user);
            if(token == null)
            {
                return new BadRequestObjectResult("Invalid credentials");
            }
            return Ok(new { Token = token });
        }

        [HttpPut("User")]
        [Authorize(Roles = "Admin")]
        public IActionResult EditUser([FromBody] DashboardUserModel user)
        {          
            _userService.UpdateUser(user);
            return new EmptyResult();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("Users")]
        public ActionResult<List<DashboardUserModel>> GetUsers()
        {
            return Ok(_userService.GetAllUsers());
        }

        [HttpDelete("User/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUser([FromRoute] string id)
        {
            _userService.DeleteUser(id);
            return new EmptyResult();
        }

    }
}
