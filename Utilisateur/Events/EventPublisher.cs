namespace Utilisateur.Events;

public class EventPublisher : IEventPublisher
{
    private readonly IRabbitMQProducer _rabbitMQProducer;

    public EventPublisher(IRabbitMQProducer rabbitMQProducer)
    {
        _rabbitMQProducer = rabbitMQProducer;
    }

    public void PublishUserCreated(UserCreatedEvent userCreatedEvent)
    {
        _rabbitMQProducer.SendMessage(userCreatedEvent, "user.created");
    }

    public void PublishUserUpdated(UserUpdatedEvent userUpdatedEvent)
    {
        _rabbitMQProducer.SendMessage(userUpdatedEvent, "user.updated");
    }

    public void PublishUserDeleted(UserDeletedEvent userDeletedEvent)
    {
        _rabbitMQProducer.SendMessage(userDeletedEvent, "user.deleted");
    }
    public void PublishUserAccessed(UserAccessedEvent userAccessedEvent)
    {
        Console.WriteLine($"[RabbitMQ] Publication de l'événement user.accessed : {userAccessedEvent.Message}");
        _rabbitMQProducer.SendMessage(userAccessedEvent, "user.accessed");
    }
}