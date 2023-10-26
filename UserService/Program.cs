using Classifieds.Auth;
using DAL;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prometheus;
using UserService.Dto;
using UserService.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<UserService.Services.UserService>();
builder.Services.AddSingleton<IPasswordHasher<UserProfile>, PasswordHasher<UserProfile>>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddHttpLogging(options => options.LoggingFields = HttpLoggingFields.All);
builder.Services.AddDbContext<ClassifiedsDbContext>();
builder.Services.AddClassifiedsAuth();

var app = builder.Build();

app.UseHttpLogging();
app.UseAuthentication();

app.UseSwagger();
app.UseSwaggerRequestInterceptor();
app.UseHttpMetrics();
app.MapMetrics();

app.UseExceptionHandler(exceptionHandlerApp
    => exceptionHandlerApp.Run(async context
        =>
    {
        var exceptionHandlerPathFeature =
            context.Features.Get<IExceptionHandlerPathFeature>();
        await Results.Problem(title: exceptionHandlerPathFeature.Error.Message)
            .ExecuteAsync(context);
    }));

app.MapGet("api/User",
        async (int userId, HttpContext ctx, [FromServices] UserService.Services.UserService userService) =>
        {
            if (IsUserHasAccess(ctx, userId)) return Results.Forbid();
            return Results.Ok(await userService.GetUser(userId));
        })
    .WithOpenApi().ProducesProblem(500);

app.MapPost("api/User",
    async (CreateUserDto user, [FromServices] UserService.Services.UserService userService) =>
    {
        return await userService.CreateUser(user);
    }).WithOpenApi().ProducesProblem(500);

app.MapPut("api/User",
    async (UpdateUserDto user, HttpContext ctx, [FromServices] UserService.Services.UserService userService) =>
    {
        if (IsUserHasAccess(ctx, user.Id)) return Results.Forbid();
        return Results.Ok(await userService.UpdateUser(user));
    }).WithOpenApi().ProducesProblem(500);

app.MapPost("api/auth/token", async ([FromBody] AuthenticateRequest authenticateRequest, [FromServices] AuthService authService) =>
{
    return await authService.GenerateAuthTokenAsync(authenticateRequest);
});


app.MapDelete("api/User", async (int userId, HttpContext ctx, [FromServices] UserService.Services.UserService userService) =>
{
    if (IsUserHasAccess(ctx, userId)) return Results.Forbid();
    await userService.DeleteUser(userId);
    return Results.StatusCode(204);
}).WithOpenApi().ProducesProblem(500);


app.Run();

static bool IsUserHasAccess(HttpContext ctx, int userId)
{
    if (ctx.User.Claims.FirstOrDefault(x => x.Type == "id")?.Value != userId.ToString()) return true;
    return false;
}