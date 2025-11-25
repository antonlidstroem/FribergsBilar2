using System.Diagnostics;
using System.Text;
using DAL.Classes;
using DAL.Interfaces;
using DAL.Repositories;
using FribergsApi.MappingProfiles;
using FribergsApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace FribergsApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Hämta connectionstring
            var connectionstring = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");

            // DbContext från DAL-projektet
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionstring));

            //// Identity
            //builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            //    options.SignIn.RequireConfirmedAccount = false)
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultTokenProviders();

            builder.Services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            builder.Services.AddScoped<TokenService>();
            builder.Services.AddScoped<RefreshTokenService>();

            // AutoMapper
            builder.Services.AddAutoMapper(cfg =>
            { }, typeof(MappingProfile));

            // DAL-repositories
            builder.Services.AddScoped<ICarRepository, CarRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            //builder.Services.AddScoped<UserService>();

            // CORS
            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowAll", builder =>
            //    {
            //        builder.AllowAnyOrigin()
            //               .AllowAnyMethod()
            //               .AllowAnyHeader();
            //    });
            //});

            // CORS 2

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                   b => b.AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowAnyOrigin());
                    });


            builder.Services.AddControllers()
              .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });


            // Swagger
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            // JWT Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                


                //options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(2),
                    ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                    ValidAudience = builder.Configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
                };

            });

            var app = builder.Build();

            //Felsökning
            var logger = app.Logger;
            logger.LogInformation("JWT Key used for validation: {key}", builder.Configuration["JwtSettings:Key"]);


            // Seedning
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await SeedData.InitializeAsync(services);
            }

            //Konfigurera HTTP-pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                    c.RoutePrefix = string.Empty;
                });
            }

            // Öppna webbläsare
            string url = "https://localhost:7251/index.html";
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error opening browser: " + ex.Message);
            }

            // Middleware
            app.UseCors("AllowAll");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
