using Steeltoe.Messaging.RabbitMQ.Core;

namespace Utilisateur.Events;

public class EventPublisher
{
    private readonly RabbitTemplate _rabbitTemplate;

    public EventPublisher(RabbitTemplate rabbitTemplate)
    {
        _rabbitTemplate = rabbitTemplate;
    }

    public void PublishUserCreated(UserCreatedEvent logEvent)
    {
        _rabbitTemplate.ConvertAndSend("ms.utilisateur", "user.created", logEvent);
        Console.WriteLine($"Événement publié : {logEvent.Message}");
    }
}