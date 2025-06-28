namespace Utilisateur.Events;

public interface IRabbitMQProducer
{
    void SendMessage<T>(T message, string routingKey);
}