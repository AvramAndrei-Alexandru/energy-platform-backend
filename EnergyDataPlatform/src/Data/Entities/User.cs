using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyDataPlatform.src.Data.Entities
{
    public class User : IdentityUser
    {
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public List<SmartDevice> SmartDevices { get; set; }
    }
}
