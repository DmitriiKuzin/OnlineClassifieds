using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Classifieds.Auth;

public static class AuthExtension
{
    public static IServiceCollection AddClassifiedsAuth(this IServiceCollection services)
    {
        services.AddAuthentication().AddJwtBearer(options =>
        {
            var key =
                "suck ass asgsfsefef afafsfrsfasf sfsfsefsfdhryjtyit6u45t23423e23d2tgr23r2r22r323r23t23y23r4wgdfgdb"u8
                    .ToArray();
            options.ClaimsIssuer = "user-service";
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = "user-service",
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });
        services.AddAuthorization(options =>
        {
            options.AddPolicy("ApiScope", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "SampleAPI");
            });
        });
        return services;
    }

    public static long GetUserId(this HttpContext ctx)
    {
        return Convert.ToInt64(ctx.User.Claims.FirstOrDefault(x => x.Type == "id")?.Value);
    }
    public static Roles GetUserRole(this HttpContext ctx)
    {
        return (Roles) int.Parse(ctx.User.Claims.FirstOrDefault(x => x.Type == "roleId")?.Value);
    }
}