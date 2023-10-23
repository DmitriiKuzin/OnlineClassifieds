using MassTransit;
using MQ;

namespace NotificationService.Consumers;

public class ModerationFailedConsumer: IConsumer<ModerationFailed>
{
    public Task Consume(ConsumeContext<ModerationFailed> context)
    {
        Console.WriteLine("Ваше объявление не удалось опубликовать, так как оно не прошло модерацию");
        return Task.CompletedTask;
    }
}