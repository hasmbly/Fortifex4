namespace Fortifex4.Application.Common.Interfaces
{
    public interface ICurrentUser
    {
        bool IsAuthenticated { get; }
        string Username { get; }
        string PictureURL { get; }
    }
}