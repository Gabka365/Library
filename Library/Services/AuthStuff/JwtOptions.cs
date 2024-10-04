namespace Library.Services.AuthStuff
{
    public class JwtOptions
    {
        public string SecretKey { get; set; }
        public int ExpiresHours { get; set; } 
    }
}
