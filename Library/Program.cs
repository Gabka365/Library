using Library.CustomMiddlewareServices;
using Library.Data;
using Library.Data.Repositories;
using Library.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<LibraryDbContext>();
builder.Services.AddScoped<AuthorsRepository>();
builder.Services.AddScoped<BooksRepository>();
builder.Services.AddSingleton<PathHelper>();

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
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
