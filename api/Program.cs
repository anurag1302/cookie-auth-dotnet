using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "ApiAuthCookie";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.SameSite = SameSiteMode.Strict;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                    options.LoginPath = "/api/login";
                    options.LogoutPath = "/api/logout";
                });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddAuthorization();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            //app.MapControllers();
            app.MapGet("/", (Func<string>)(() => "Hello World!"));

            app.MapPost("/api/login", async (HttpContext context, string username, string password, string ReturnUrl) =>
            {
                // Replace this with your actual user validation logic
                if (username == "test" && password == "password")
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, username),
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    context.Response.StatusCode = StatusCodes.Status200OK;
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                }
            });

            app.MapPost("/api/logout", async (HttpContext context) =>
            {
                await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                context.Response.StatusCode = StatusCodes.Status200OK;
            });

            app.MapGet("/api/protected", () => "Protected data").RequireAuthorization();

            app.Run();
        }
    }
}