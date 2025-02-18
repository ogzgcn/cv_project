using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Logimat.API.Models;
using Microsoft.IdentityModel.Tokens;

namespace Logimat.API.Services;

public class JwtService
{
    private readonly IConfiguration _config;

    public JwtService(IConfiguration config)
    {
        _config = config;
    }

   public string GenerateToken(ApplicationUser user)
{
    var claims = new[]
    {
        new Claim(ClaimTypes.Name, user.UserName),
    };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    
    var token = new JwtSecurityToken(
        _config["JwtSettings:Issuer"],
        _config["JwtSettings:Audience"],
        claims,
        expires: DateTime.Now.AddHours(1),
        signingCredentials: creds);

    return new JwtSecurityTokenHandler().WriteToken(token);
}

}