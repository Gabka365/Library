﻿using Library.Data.Models;
using Library.Data.Repositories;
using Library.Models.Auth;
using Library.Services.AuthStuff;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    public class AuthController : Controller
    {
        private PasswordHasher _passwordHasher;
        private UserRepository _userRepository;
        private JwtProvider _jwtProvider;

        public AuthController(PasswordHasher passwordHasher, UserRepository userRepository, JwtProvider jwtProvider) 
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserPersonalViewModel viewModel)
        {
            var hashedPassword = _passwordHasher.Generate(viewModel.Password);

            var user = new User
            {
                Name = viewModel.UserName,
                Password = hashedPassword
            };

            _userRepository.Create(user);

            return View(); 
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserPersonalViewModel viewModel)
        {
            var user = _userRepository.GetByUsername(viewModel.UserName);
            if (user is null)
            {
                return View(viewModel);
            }

            var result = _passwordHasher.Verify(viewModel.Password, user.Password);
            if (result is false)
            {
                throw new Exception("Failed to login");
            }

            var token = _jwtProvider.GenerateToken(user);

            HttpContext.Response.Cookies.Append("nice-value", token);

            return View();
        }
    }
}
