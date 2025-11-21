using System.Net.Http.Headers;
using MarcusRent.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using FribergsApi.MappingProfiles;
using MarcusRent.Interfaces;


namespace MarcusRent
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Lägg till IHttpContextAccessor för att komma åt sessionen
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //Http-klienter
            builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7251/");
            });

            builder.Services.AddHttpClient<ICarApiRepository, CarApiRepository>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7251/");
            });

            builder.Services.AddHttpClient<IOrderApiRepository, OrderApiRepository>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7251/"); // Lägg till BaseAddress här
            });

            builder.Services.AddHttpClient<IUserApiRepository, UserApiRepository>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7251/");
            });

            builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile));  



            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(1);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();
            app.Run();
        }
    }
}
