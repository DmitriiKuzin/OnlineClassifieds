using DAL;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using MQ;

namespace NotificationService.Consumers;

public class ModerationFailedConsumer : IConsumer<ModerationFailed>
{
    private readonly ClassifiedsDbContext _dbContext;

    public ModerationFailedConsumer(ClassifiedsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<ModerationFailed> context)
    {
        var listing = await _dbContext
            .Listings.AsNoTracking()
            .FirstAsync(x => x.Id == context.Message.ListingId);
        
        _dbContext.Notifications.Add(new Notification
        {
            UserProfileId = context.Message.UserProfileId,
            Message = $"Ваше объявление \"{listing.Title}\" не удалось опубликовать, так как оно не прошло модерацию"
        });
        await _dbContext.SaveChangesAsync();
    }
}