using Classifieds.Auth;
using DAL;
using Microsoft.EntityFrameworkCore;
using MQ;
using NotificationService.Consumers;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ClassifiedsDbContext>();
builder.Services.AddRabbitMq(x =>
{
    x.AddConsumer<ListingPublishRequestedConsumer>();
    x.AddConsumer<ListingPublishedConsumer>();
    x.AddConsumer<ModerationFailedConsumer>();
});
builder.Services.AddClassifiedsAuth();
builder.Services.AddSwaggerWithAuth();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerRequestInterceptor();
app.UseAuthentication();
app.UseAuthorization();
app.MapGet("getMyNotifications", async (HttpContext ctx, ClassifiedsDbContext dbContext) =>
{
    if (ctx.GetUserId() == 0) return Results.Forbid();
    var notifications = await dbContext
        .Notifications
        .Where(x => x.UserProfileId == ctx.GetUserId())
        .Select(x => x.Message)
        .ToListAsync();
    return Results.Json(notifications);
});


await MqExtension.WaitForRabbitReady();
app.Run();