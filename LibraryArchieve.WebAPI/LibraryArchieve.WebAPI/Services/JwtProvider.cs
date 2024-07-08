using LibraryArchieve.WebAPI.Data.Entities;
using LibraryArchieve.WebAPI.V1.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibraryArchieve.WebAPI.Services;

public class JwtProvider(UserManager<AppUser> userManager)
{
    public async Task<LoginResponse> CreateToken(AppUser appUser, List<string?>? roles)
    {
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString()),
            new Claim(ClaimTypes.Name, appUser.FullName),
        };

        DateTime expires = DateTime.UtcNow.AddHours(1);

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("my secret key my secret key my secret key 1234...my secret key my secret key my secret key 1234..."));

        JwtSecurityToken jwtSecurityToken = new(
            issuer: "Çağla Tunç Savaş",
            audience: "Task Çözümü",
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expires,
            signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512));

        JwtSecurityTokenHandler handler = new();

        string token = handler.WriteToken(jwtSecurityToken);

        string refreshToken = Guid.NewGuid().ToString();

        DateTime refreshTokenExpires = DateTime.UtcNow.AddMonths(1);

        await userManager.UpdateAsync(appUser);

        return new LoginResponse
        {
            Token = token,
            RefreshToken = Guid.NewGuid().ToString(),
            RefreshTokenExpires = DateTime.UtcNow.AddMonths(1)
        };

    }
}
