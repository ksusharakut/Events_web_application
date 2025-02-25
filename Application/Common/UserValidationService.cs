using Application.Common;

namespace Infrastructure.Services
{
    public class UserValidationService : IUserValidationService
    {
        public bool IsUserOldEnough(DateTime dateOfBirth)
        {
            var today = DateTime.UtcNow;
            var age = today.Year - dateOfBirth.Year;

            if (dateOfBirth.Date > today.AddYears(-age)) age--;

            return age >= 18;
        }
    }
}
