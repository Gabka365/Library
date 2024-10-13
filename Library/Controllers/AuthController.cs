using Library.Data.Models;
using Library.Data.Repositories.Interfaces;
using Library.Models.Auth;
using Library.Services.AuthStuff.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Library.Data.Enums;

namespace Library.Controllers
{
    public class AuthController : Controller
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly IRefreshTokenProvider _refreshTokenProvider;

        public AuthController(IPasswordHasher passwordHasher, IUserRepository userRepository, 
            IJwtProvider jwtProvider, IRefreshTokenProvider refreshTokenProvider) 
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _refreshTokenProvider = refreshTokenProvider;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var hashedPassword = _passwordHasher.Generate(viewModel.Password);

            var user = new User
            {
                Name = viewModel.UserName,
                HashedPassword = hashedPassword,
                UserRole = UserRole.User
            };

            _userRepository.Create(user);

            var token = _jwtProvider.GenerateToken(user);
            HttpContext.Response.Cookies.Append("nice-value", token);
            var refreshToken = _refreshTokenProvider.GenerateRefreshToken();
            _refreshTokenProvider.SetRefreshToken(refreshToken, user);

            return RedirectToAction("TokenCheck", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(AuthViewModel viewModel)
        {
            var user = _userRepository.GetByUsername(viewModel.UserName);
            if (user is null)
            {
                return View(viewModel);
            }

            var result = _passwordHasher.Verify(viewModel.Password, user.HashedPassword);
            if (result is false)
            {
                return View(viewModel);
            }

            var token = _jwtProvider.GenerateToken(user);
            HttpContext.Response.Cookies.Append("nice-value", token);
            var refreshToken = _refreshTokenProvider.GenerateRefreshToken();
            _refreshTokenProvider.SetRefreshToken(refreshToken, user);


            return RedirectToAction("TokenCheck","Home");
        }


        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("nice-value");
            return RedirectToAction("Login");
        }
    }
}
