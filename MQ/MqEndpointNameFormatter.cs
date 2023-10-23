using MassTransit;

namespace MQ;

public class MqEndpointNameFormatter: DefaultEndpointNameFormatter
{
    protected override string GetConsumerName(Type type)
    {
        if (type.IsGenericType && type.Name.Contains('`'))
            return SanitizeName(FormatName(type.GetGenericArguments().Last()));
        var prefix = type.Assembly.GetName().Name;

        const string consumer = "Consumer";

        var consumerName = FormatName(type);

        if (consumerName.EndsWith(consumer, StringComparison.InvariantCultureIgnoreCase))
            consumerName = consumerName.Substring(0, consumerName.Length - consumer.Length);

        return SanitizeName(prefix + "_" + consumerName);
    }
}