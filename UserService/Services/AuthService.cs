using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UserService.Dto;

namespace UserService.Services;

public class AuthService
{
    private readonly ClassifiedsDbContext _context;
    private readonly IPasswordHasher<UserProfile> _passwordHasher;

    public AuthService(ClassifiedsDbContext context, IPasswordHasher<UserProfile> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<string> GenerateAuthTokenAsync(AuthenticateRequest model)
    {
        var user = await _context
            .UserProfiles
            .AsNoTracking()
            .FirstAsync(x => x.PhoneNumber == model.PhoneNumber);

        // return null if user not found
        if (user == null) return null;

        var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);

        if (verificationResult == PasswordVerificationResult.Failed) return null;
        
        // authentication successful so generate jwt token
        var token = generateJwtToken(user);

        return token;
    }
    
    private string generateJwtToken(UserProfile user)
    {
        // generate token that is valid for 7 days
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("suck ass asgsfsefef afafsfrsfasf sfsfsefsfdhryjtyit6u45t23423e23d2tgr23r2r22r323r23t23y23r4wgdfgdb");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()), new Claim("roleId", user.RoleId.ToString()) }),
            Issuer = "user-service",
            Expires = DateTime.UtcNow.AddMinutes(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}