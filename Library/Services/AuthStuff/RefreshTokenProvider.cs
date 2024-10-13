using Azure;
using Library.Data.Repositories;
using Library.Data.Models;
using System.Security.Cryptography;
using Library.Services.AuthStuff.Interfaces;
using Library.Data.Repositories.Interfaces;

namespace Library.Services.AuthStuff
{
    public class RefreshTokenProvider : IRefreshTokenProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;

        public RefreshTokenProvider(IHttpContextAccessor httpContextAccessor, IAuthService authService
            , IUserRepository userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _authService = authService;
            _userRepository = userRepository;
        }

        public RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7)
            };

            return refreshToken;
        }

        public void SetRefreshToken(RefreshToken newRefreshToken, User user)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires,
            };
            _httpContextAccessor.HttpContext!.Response.Cookies.Append
                ("refreshToken", newRefreshToken.Token, cookieOptions);

            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;
            _userRepository.Update(user);
        }
    }
}
