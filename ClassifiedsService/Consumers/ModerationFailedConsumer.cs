using DAL;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using MQ;

namespace ClassifiedsService.Consumers;

public class ModerationFailedConsumer: IConsumer<ModerationFailed>
{
    private readonly ClassifiedsDbContext _dbContext;

    public ModerationFailedConsumer(ClassifiedsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<ModerationFailed> context)
    {
        var listing = await _dbContext.Listings.FirstAsync(x => x.Id == context.Message.ListingId);
        listing.Status = ListingStatus.Published;
        await _dbContext.SaveChangesAsync();
    }
}