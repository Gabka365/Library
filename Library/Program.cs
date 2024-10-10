using Library.CustomMiddlewareServices;
using Library.Data;
using Library.Data.Repositories;
using Library.Models.ValidationAttributes;
using Library.Services;
using Library.Services.AuthStuff;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<LibraryDbContext>();
builder.Services.AddScoped<AuthorsRepository>();
builder.Services.AddScoped<BooksRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<BookInstancesRepository>();
builder.Services.AddSingleton<PathHelper>();
builder.Services.AddScoped<PasswordHasher>();
builder.Services.AddScoped<JwtProvider>();
builder.Services.AddScoped<RefreshTokenProvider>();
builder.Services.AddScoped<AuthService>();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));


var jwtOptions = builder.Configuration.GetSection("JwtOptions").Get<JwtOptions>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["nice-value"];

                return Task.CompletedTask;
            }
        };

    });
builder.Services.AddAuthorization();


var app = builder.Build();
var seed = new Seed();
seed.Fill(app.Services);
app.UseMiddleware<GlobalExceptionHandler>();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");
app.Run();
