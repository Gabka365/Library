using Library.Data.Repositories;
using Library.Services.AuthStuff;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Library.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AuthService _authService;
        private readonly JwtProvider _jwtProvider;
        private readonly RefreshTokenProvider _refreshTokenProvider;


        public HomeController(ILogger<HomeController> logger, AuthService authService, JwtProvider jwtProvider, RefreshTokenProvider refreshTokenProvider)
        {
            _logger = logger;
            _authService = authService;
            _jwtProvider = jwtProvider;
            _refreshTokenProvider = refreshTokenProvider;
        }

        public IActionResult Index()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var user = _authService.GetUser();

            if(!user.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token");
            }
            else if(user.TokenExpires < DateTime.Now)
            {
                string token = _jwtProvider.GenerateToken(user);
                var newRefreshToken = _refreshTokenProvider.GenerateRefreshToken();
                _refreshTokenProvider.SetRefreshToken(newRefreshToken, user);
            }

            return View();
        }
        
        public IActionResult Books()
        {
            return View();
        }
        
        public IActionResult Authors()
        {
            return View();
        }

    }
}
