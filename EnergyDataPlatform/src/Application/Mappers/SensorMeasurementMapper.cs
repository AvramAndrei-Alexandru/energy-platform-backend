using EnergyDataPlatform.src.Application.Models;
using EnergyDataPlatform.src.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyDataPlatform.src.Application.Mappers
{
    public static class SensorMeasurementMapper
    {
        public static SensorMeasurement ToSensorMeasurementEntity(SensorMeasurementDashboardModel sensorMeasurement)
        {
            if(sensorMeasurement == null)
            {
                return null;
            }

            return new SensorMeasurement
            {
                Id = sensorMeasurement.Id != null ? sensorMeasurement.Id.Value : Guid.NewGuid(),
                Timestamp = sensorMeasurement.Timestamp,
                Measurement = sensorMeasurement.Measurement,
                SmartDeviceId = sensorMeasurement.SmartDeviceId
            };
        }

        public static SensorMeasurementDashboardModel ToSensorMeasurementDashboardModel(SensorMeasurement sensorMeasurement)
        {
            if(sensorMeasurement == null)
            {
                return null;
            }

            return new SensorMeasurementDashboardModel
            {
                Id = sensorMeasurement.Id,
                Timestamp = sensorMeasurement.Timestamp,
                Measurement = sensorMeasurement.Measurement,
                SmartDeviceId = sensorMeasurement.SmartDeviceId
            };
        }
    }
}