using DAL;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using MQ;

namespace NotificationService.Consumers;

public class ListingPublishedConsumer: IConsumer<ListingPublished>
{
    private readonly ClassifiedsDbContext _dbContext;

    public ListingPublishedConsumer(ClassifiedsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<ListingPublished> context)
    {
        var listing = await _dbContext
            .Listings.AsNoTracking()
            .FirstAsync(x => x.Id == context.Message.ListingId);
        
        _dbContext.Notifications.Add(new Notification
        {
            UserProfileId = context.Message.UserProfileId,
            Message = $"Ваше объявление \"{listing.Title}\" опубликовано. Теперь его могут видеть другие пользователи"
        });
        await _dbContext.SaveChangesAsync();
    }
}