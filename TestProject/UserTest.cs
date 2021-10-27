using EnergyDataPlatform.src.Application.Services.Implementation;
using EnergyDataPlatform.src.Application.Services.Interfaces;
using EnergyDataPlatform.src.Data.Repositories.Implementation;
using EnergyDataPlatform.src.Data.Repositories.Interfaces;
using Moq;
using System;
using Xunit;

namespace TestProject
{
    public class UserTest
    {

        private IUserService _userService;
        private Mock<IUserRepository> _userRepository;

        [Fact]
        public void TestUserNameExistsMusteReturnTrue()
        {
            _userRepository = new Mock<IUserRepository>();
            _userRepository.Setup(u => u.ExistsUser("Andrei")).Returns(true);
            _userService = new UserService(null, _userRepository.Object, null);

            Assert.True(_userService.ExistsUser("Andrei"));
        }

        [Fact]
        public void TestUserNameExistsMusteReturnFalse()
        {
            _userRepository = new Mock<IUserRepository>();
            _userRepository.Setup(u => u.ExistsUser("Andrei")).Returns(true);
            _userService = new UserService(null, _userRepository.Object, null);

            Assert.False(_userService.ExistsUser("Andrei1"));
        }
    }
}
