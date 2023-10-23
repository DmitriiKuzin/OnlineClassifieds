using DAL;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using MQ;

namespace NotificationService.Consumers;

public class ListingPublishedConsumer: IConsumer<ListingPublished>
{
    public Task Consume(ConsumeContext<ListingPublished> context)
    {
        Console.WriteLine("Ваше объявление опубликовано. Теперь его могут видеть другие пользователи");
        return Task.CompletedTask;
    }
}