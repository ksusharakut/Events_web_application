using Application.Common;

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
