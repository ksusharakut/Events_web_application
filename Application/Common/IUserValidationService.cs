namespace Application.Common
{
    public interface IUserValidationService
    {
        bool IsUserOldEnough(DateTime birthDay);
    }
}
