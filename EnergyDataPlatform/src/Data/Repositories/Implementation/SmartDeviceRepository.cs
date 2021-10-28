using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnergyDataPlatform.src.Data.Entities;
using EnergyDataPlatform.src.Data.Persistance;
using EnergyDataPlatform.src.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EnergyDataPlatform.src.Data.Repositories.Implementation
{
    public class SmartDeviceRepository : ISmartDeviceRepository
    {
        private readonly DatabaseContext _context;

        public SmartDeviceRepository(DatabaseContext context)
        {
            _context = context;
        }

        public void AddDevice(SmartDevice smartDevice)
        {
            smartDevice.Id = Guid.NewGuid();
            _context.SmartDevices.Add(smartDevice);
            _context.SaveChanges();
        }

        public List<SmartDevice> GetAllDevicesForUser(string userId)
        {
            return _context.SmartDevices.OrderBy(d => d.Description).Include(s => s.SensorMeasurements).Where(d => d.UserId == userId).ToList();
        }

        public SmartDevice GetDeviceById(Guid id)
        {
            return _context.SmartDevices.Include(s => s.SensorMeasurements).FirstOrDefault(s => s.Id == id);
        }

        public decimal GetTotalEnergyConsumptionForDevice(Guid id)
        {
            var device = _context.SmartDevices.Include(d => d.SensorMeasurements).FirstOrDefault(d => d.Id == id);
            var totalEnergy = decimal.Zero;
            if(device.SensorMeasurements != null && device.SensorMeasurements.Count != 0)
            {
                foreach (var measurement in device.SensorMeasurements)
                {
                    totalEnergy += measurement.Measurement;
                }
            }
            return totalEnergy;
        }

        public void RemoveDevice(SmartDevice smartDevice)
        {
            var device = _context.SmartDevices.Include(s => s.SensorMeasurements).FirstOrDefault(d => d.Id == smartDevice.Id);
            _context.SensorMeasurements.RemoveRange(device.SensorMeasurements);
            _context.SmartDevices.Remove(smartDevice);
            _context.SaveChanges();
        }

        public void UpdateDevice(SmartDevice smartDevice)
        {
            var dbSmartDevice = _context.SmartDevices.FirstOrDefault(s => s.Id == smartDevice.Id);

            if(dbSmartDevice != null)
            {
                dbSmartDevice.Description = smartDevice.Description;
                dbSmartDevice.SensorDescription = smartDevice.SensorDescription;
                dbSmartDevice.Address = smartDevice.Address;
                dbSmartDevice.MaximumEnergyConsumption = smartDevice.MaximumEnergyConsumption;
            }
            _context.SaveChanges();
        }
    }
}