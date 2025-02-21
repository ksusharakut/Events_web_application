namespace Application.Common
{
    public interface ICurrentUserService
    {
        int UserId { get; }
        string Role { get; }
    }
}
