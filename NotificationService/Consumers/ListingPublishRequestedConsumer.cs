using DAL;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using MQ;

namespace NotificationService.Consumers;

public class ListingPublishRequestedConsumer : IConsumer<ListingPublishRequested>
{
    private readonly ClassifiedsDbContext _dbContext;

    public ListingPublishRequestedConsumer(ClassifiedsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<ListingPublishRequested> context)
    {
        var listing = await _dbContext
            .Listings.AsNoTracking()
            .FirstAsync(x => x.Id == context.Message.ListingId);
        
        _dbContext.Notifications.Add(new Notification
        {
            UserProfileId = context.Message.UserProfileId,
            Message = $"Ваше объявление \"{listing.Title}\" находится на модерации и в скором времени будет опубликовано"
        });
        await _dbContext.SaveChangesAsync();
    }
}