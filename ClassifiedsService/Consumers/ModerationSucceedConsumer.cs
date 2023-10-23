using DAL;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using MQ;

namespace ClassifiedsService.Consumers;

public class ModerationSucceedConsumer: IConsumer<ModerationSucceed>
{
    private readonly ClassifiedsDbContext _dbContext;
    private readonly IBus _bus;

    public ModerationSucceedConsumer(ClassifiedsDbContext dbContext, IBus bus)
    {
        _dbContext = dbContext;
        _bus = bus;
    }

    public async Task Consume(ConsumeContext<ModerationSucceed> context)
    {
        var listing = await _dbContext.Listings.FirstAsync(x => x.Id == context.Message.ListingId);
        listing.Status = ListingStatus.Published;
        await _dbContext.SaveChangesAsync();
        await _bus.Publish(new ListingPublished(context.Message.ListingId));
    }
}