using Library.Data.Enums;
using Library.Data.Models;
using Library.Data.Repositories;

namespace Library.Services.AuthStuff
{
    public class AuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserRepository _userRepository; 

        public AuthService(IHttpContextAccessor httpContextAccessor, UserRepository userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        public User GetUser()
        {
            var userId = GetUserId();
            User user = _userRepository.Get(userId)!;
            return user;
        }

        public int GetUserId()
        {
            var userIdText = GetClaimValue("userId");
            var userId = int.Parse(userIdText);
            return userId;
        }

        public UserRole GetUserRole()
        {
            var userRole = GetClaimValue("userRole");
            return Enum.Parse<UserRole>(userRole);
        }

        public bool IsAdmin()
        {
            return IsAuthenticated() && GetUserRole() == UserRole.Admin;
        }

        public bool IsAuthenticated()
        {
            return _httpContextAccessor.HttpContext!.User.Identity?.IsAuthenticated ?? false;
        }

        private string GetClaimValue(string claimType) 
         => _httpContextAccessor
            .HttpContext!
            .User
            .Claims
            .First(x => x.Type == claimType)
            .Value;
    }
}
