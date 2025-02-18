using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Logimat.API.Models;
using Logimat.API.Services;

namespace Logimat.API.Endpoints
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this WebApplication app)
        {
            app.MapPost("/register", async ([FromBody] RegisterModel model, UserManager<ApplicationUser> userManager) =>
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FullName = model.FullName };
                var result = await userManager.CreateAsync(user, model.Password);
                
                return result.Succeeded ? Results.Ok("User Created") : Results.BadRequest(result.Errors);
            });

            app.MapPost("/login", async ([FromBody] LoginModel model, UserManager<ApplicationUser> userManager, JwtService jwtService) =>
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null || !await userManager.CheckPasswordAsync(user, model.Password))
                    return Results.Unauthorized();

                var token = jwtService.GenerateToken(user);
                return Results.Ok(new { Token = token });
            });
        }
    }

    public record RegisterModel(string FullName, string Email, string Password);
    public record LoginModel(string Email, string Password);
}