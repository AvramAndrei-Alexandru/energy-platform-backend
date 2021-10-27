using EnergyDataPlatform.src.Application.Enums;
using EnergyDataPlatform.src.Application.Mappers;
using EnergyDataPlatform.src.Application.Models;
using EnergyDataPlatform.src.Application.Services.Interfaces;
using EnergyDataPlatform.src.Data.Entities;
using EnergyDataPlatform.src.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EnergyDataPlatform.src.Application.Services.Implementation
{
    public class UserService : IUserService
    {

        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly SignInManager<User> _signInManager;

        public UserService(UserManager<User> userManager, IUserRepository userRepository, SignInManager<User> signInManager) 
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _signInManager = signInManager;
        }

        public void DeleteUser(string id)
        {
            _userRepository.DeleteUser(id);
        }

        public bool ExistsUser(string userName)
        {
            return _userRepository.ExistsUser(userName);
        }

        public List<DashboardUserModel> GetAllUsers()
        {
            return _userRepository.GetAllUsers().Select(u => UserMapper.ToDashboardUserModel(u, _userManager.GetRolesAsync(u).Result)).ToList();
        }

        public string Login(LoginUserModel userModel)
        {
            try
            { 
                if (userModel != null)
                {
                    var existingUser =  _userManager.FindByNameAsync(userModel.UserName).Result;

                    if (existingUser != null)
                    {
                        var result =  _signInManager.PasswordSignInAsync(existingUser, userModel.Password, false, false).Result;

                        if (result.Succeeded)
                        {

                            var userRoles =  _userManager.GetRolesAsync(existingUser).Result;
                            var claim = new Claim(new IdentityOptions().ClaimsIdentity.RoleClaimType, userRoles[0]);
                            var listClaims = new List<Claim>
                            {
                                claim
                            };
                            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mySecretKey1234@2021.89"));
                            var signingInCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                            var tokenOptions = new JwtSecurityToken(
                                issuer: "https://localhost:44353",
                                claims: listClaims,
                                expires: DateTime.Now.AddHours(1),
                                signingCredentials: signingInCredentials
                                );
                            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                        }
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<IdentityError> Register(UserModel userModel)
        {
            var addUserResult = _userManager.CreateAsync(UserMapper.ToUserEntity(userModel), userModel.Password);

            if(addUserResult.Result != IdentityResult.Success)
            {
                return addUserResult.Result.Errors;
            }

            var registeredUser = _userManager.FindByNameAsync(userModel.UserName);

            var addRoleResult = _userManager.AddToRoleAsync(registeredUser.Result, userModel.Role.ToString());

            if(addRoleResult.Result != IdentityResult.Success)
            {
                return addRoleResult.Result.Errors;
            }

            return null;

        }

        public void UpdateUser(DashboardUserModel user)
        {
            _userRepository.UpdateUser(UserMapper.ToUserEntity(user));
        }
    }
}
