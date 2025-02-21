using Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AgeValidationService : IAgeValidationService
    {
        public bool IsAtLeast18YearsOld(DateTime birthDay)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDay.Year;

            if (birthDay > today.AddYears(-age))
            {
                age--;
            }

            return age >= 18;
        }
    }
}
