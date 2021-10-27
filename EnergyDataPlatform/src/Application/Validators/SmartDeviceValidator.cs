using EnergyDataPlatform.src.Application.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyDataPlatform.src.Application.Validators
{
    public class SmartDeviceValidator : AbstractValidator<SmartDeviceDashboardModel>
    {
        public SmartDeviceValidator()
        {
            RuleFor(s => s.MaximumEnergyConsumption).Must(BePositive).WithMessage("Maximum energy consumption must be positive");
        }
        private bool BePositive(decimal value)
        {
            return value >= 0;
        }
    }
}
