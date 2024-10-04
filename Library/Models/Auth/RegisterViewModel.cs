using Library.Models.ValidationAttributes;

namespace Library.Models.Auth
{
    public class RegisterViewModel
    {
        [UserCheck]
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
