using EnergyDataPlatform.src.Application.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyDataPlatform.src.Application.Validators
{
    public class SensorMeasurementValidator : AbstractValidator<SensorMeasurementDashboardModel>
    {
        public SensorMeasurementValidator()
        {
            RuleFor(s => s.Measurement).Must(BePositive).WithMessage("Measurement must be positive");
        }

        private bool BePositive(decimal value)
        {
            return value >= 0;
        }
    }
}
