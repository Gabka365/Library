using Library.Data.Repositories;
using Library.Models.Home;
using Library.Services.AuthStuff.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Library.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAuthService _authService;
        private readonly IJwtProvider _jwtProvider;
        private readonly IRefreshTokenProvider _refreshTokenProvider;


        public HomeController(ILogger<HomeController> logger, IAuthService authService, 
            IJwtProvider jwtProvider, IRefreshTokenProvider refreshTokenProvider)
        {
            _logger = logger;
            _authService = authService;
            _jwtProvider = jwtProvider;
            _refreshTokenProvider = refreshTokenProvider;
        }
        
        [Authorize]
        public IActionResult Index()
        {
            var user = _authService.GetUser();

            var viewModel = new IndexViewModel
            {
                UserName = user.Name,
            };

            return View(viewModel);
        }

        [Authorize]
        public IActionResult TokenCheck()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var user = _authService.GetUser();

            if (!user.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token");
            }
            else if (user.TokenExpires < DateTime.Now)
            {
                string token = _jwtProvider.GenerateToken(user);
                HttpContext.Response.Cookies.Append("nice-value", token);
                var newRefreshToken = _refreshTokenProvider.GenerateRefreshToken();
                _refreshTokenProvider.SetRefreshToken(newRefreshToken, user);
            }

            return RedirectToAction("Index");
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
