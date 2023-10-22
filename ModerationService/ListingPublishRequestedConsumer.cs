using DAL;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using MQ;

namespace ModerationService;

public class ListingPublishRequestedConsumer: IConsumer<ListingPublishRequested>
{
    private readonly ClassifiedsDbContext _dbContext;

    public ListingPublishRequestedConsumer(ClassifiedsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<ListingPublishRequested> context)
    {
        var listing = await _dbContext.Listings.FirstAsync(x => x.Id == context.Message.ListingId);
        listing.Status = ListingStatus.Moderation;
        await _dbContext.SaveChangesAsync();
    }
}