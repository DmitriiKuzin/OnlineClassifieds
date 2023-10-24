using Classifieds.Auth;
using ClassifiedsService;
using ClassifiedsService.Consumers;
using DAL;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MQ;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();


builder.Services.AddDbContext<ClassifiedsDbContext>();
builder.Services.AddClassifiedsAuth();
builder.Services.AddRabbitMq(x =>
{
    x.AddConsumer<ModerationSucceedConsumer>();
    x.AddConsumer<ModerationFailedConsumer>();
});
builder.Services.AddSwaggerWithAuth();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/createListing",
    async (HttpContext ctx, ListingDto listingDto, IBus bus, ClassifiedsDbContext dbContext) =>
    {
        if (ctx.GetUserId() == 0) return Results.Forbid();
        var model = listingDto.ToDbModel();
        model.UserProfileId = ctx.GetUserId();
        dbContext.Listings.Add(model);
        await dbContext.SaveChangesAsync();
        await bus.Publish(new ListingPublishRequested(model.Id, model.UserProfileId));
        return Results.Ok();
    });

app.MapPost("/publishListing", async (HttpContext ctx, long listingId, IBus bus, ClassifiedsDbContext dbContext) =>
{
    var listing = await dbContext.Listings.AsNoTracking()
        .FirstOrDefaultAsync(x => x.UserProfileId == ctx.GetUserId() && x.Id == listingId);
    if (listing == null) return StatusCodes.Status403Forbidden;
    if (listing.Status != ListingStatus.Created) return StatusCodes.Status400BadRequest;
    await bus.Publish(new ListingPublishRequested(listingId, listing.UserProfileId));
    return StatusCodes.Status201Created;
});

app.MapGet("/getMyListings", async (HttpContext ctx, ClassifiedsDbContext dbContext) =>
{
    var listings = await dbContext
        .Listings
        .AsNoTracking()
        .Where(x => x.UserProfileId == ctx.GetUserId())
        .ToDtoList()
        .ToListAsync();
    return listings;
});

app.MapGet("/categories", async (ClassifiedsDbContext dbContext) =>
{
    var categories = await dbContext
        .Categories
        .AsNoTracking()
        .ToListAsync();
    return categories;
});

app.MapPost("/search", async (Filter filter, ClassifiedsDbContext dbContext) =>
{
    var listingsQuery = dbContext
        .Listings
        .AsNoTracking()
        .Where(x => x.Status == ListingStatus.Published);

    if (filter.CategoriesIds != null && filter.CategoriesIds.Any())
    {
        listingsQuery = listingsQuery.Where(x => filter.CategoriesIds.Contains(x.CategoryId));
    }

    if (!filter.Term.IsNullOrEmpty())
    {
        listingsQuery = listingsQuery.Where(x => EF.Functions.ILike(x.Title, $"%{filter.Term}%"));
    }

    listingsQuery = filter.Ordering == Ordering.Asc
        ? listingsQuery.OrderBy(x => x.Price)
        : listingsQuery.OrderBy(x => x.Price);

    var listings = await listingsQuery.ToDtoList().ToListAsync();
    return listings;
});

await MqExtension.WaitForRabbitReady();
app.Run();