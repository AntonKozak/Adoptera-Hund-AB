using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using adoptera_hund.api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace adoptera_hund.api.Services;

public class TokenService
{
        private readonly UserManager<UserModel> _userManager;
        private readonly IConfiguration _configuration;
        
    public TokenService(UserManager<UserModel> userManager, IConfiguration configuration)
    {
        _configuration = configuration;  
        _userManager = userManager;
    }

    public async Task<string> CreateToken(UserModel user)
    {
        //   JWT payload...   //
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim("FirstName", user.FirstName),
            new Claim("LastName", user.LastName)
        };

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        //   JWT payload... done  //
        // Signing credentials    //
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["tokenSettings:tokenKey"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        // Configuration token //
        var options = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: DateTime.Now.AddDays(5),
            signingCredentials: credentials
        );

        // take it like a string //

        return new JwtSecurityTokenHandler().WriteToken(options);
    }
}
