using Classifieds.Auth;
using DAL;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using ModerationService;
using ModerationService.Consumers;
using MQ;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ClassifiedsDbContext>();
builder.Services.AddRabbitMq(x =>
{
    x.AddConsumer<ListingPublishRequestedConsumer>();
});
builder.Services.AddClassifiedsAuth();
builder.Services.AddSwaggerWithAuth();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.UseAuthorization();

app.MapGet("/listingsOnModeration", async (HttpContext ctx, ClassifiedsDbContext dbContext) =>
{
    if (ctx.GetUserRole() is not Roles.Moderator and Roles.Admin) return Results.Forbid();
    var listings = await dbContext
        .Listings
        .AsNoTracking()
        .Where(x => x.Status == ListingStatus.Moderation)
        .ToDtoList()
        .ToListAsync();
    return Results.Json(listings);
});

app.MapPatch("/acceptListingModeration", async (HttpContext ctx, long listingId, ClassifiedsDbContext dbContext, IBus bus) =>
{
    if (ctx.GetUserRole() is not Roles.Moderator and Roles.Admin) return Results.Forbid();
    var listing = await dbContext
        .Listings
        .FirstAsync(x => x.Id == listingId);
    listing.Status = ListingStatus.ModerationSucceed;
    await dbContext.SaveChangesAsync();
    await bus.Publish(new ModerationSucceed(listingId));
    return Results.Ok();
});

app.MapPatch("/declineListingModeration", async (HttpContext ctx, long listingId, ClassifiedsDbContext dbContext, IBus bus) =>
{
    if (ctx.GetUserRole() is not Roles.Moderator and Roles.Admin) return Results.Forbid();
    var listing = await dbContext
        .Listings
        .FirstAsync(x => x.Id == listingId);
    listing.Status = ListingStatus.ModerationFailed;
    await dbContext.SaveChangesAsync();
    await bus.Publish(new ModerationFailed(listingId));
    return Results.Ok();
});

await MqExtension.WaitForRabbitReady();
app.Run();