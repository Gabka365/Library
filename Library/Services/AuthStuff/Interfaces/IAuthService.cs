using Library.Data.Enums;
using Library.Data.Models;

namespace Library.Services.AuthStuff.Interfaces
{
    public interface IAuthService
    {
        User GetUser();
        int GetUserId();
        UserRole GetUserRole();
        bool IsAdmin();
        bool IsAuthenticated();
    }
}