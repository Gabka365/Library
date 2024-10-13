using Library.Data.Models;

namespace Library.Services.AuthStuff.Interfaces
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}