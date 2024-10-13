using Library.Data.Models;

namespace Library.Services.AuthStuff.Interfaces
{
    public interface IRefreshTokenProvider
    {
        RefreshToken GenerateRefreshToken();
        void SetRefreshToken(RefreshToken newRefreshToken, User user);
    }
}