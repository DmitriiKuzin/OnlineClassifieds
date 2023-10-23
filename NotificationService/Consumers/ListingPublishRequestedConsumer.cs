using DAL;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using MQ;

namespace NotificationService.Consumers;

public class ListingPublishRequestedConsumer: IConsumer<ListingPublishRequested>
{
    public Task Consume(ConsumeContext<ListingPublishRequested> context)
    {
        Console.WriteLine("Ваше объявление находится на модерации и в скором времени будет опубликовано");
        return Task.CompletedTask;
    }
}