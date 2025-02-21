namespace Application.Common
{
    public interface IAgeValidationService
    {
        bool IsAtLeast18YearsOld(DateTime birthDay);
    }
}
