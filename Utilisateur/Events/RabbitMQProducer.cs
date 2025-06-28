using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Utilisateur.Events;

public class RabbitMQProducer : IRabbitMQProducer
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMQProducer()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "rabbitmq",
            UserName = "myuser",
            Password = "mypassword"
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(exchange: "ms.utilisateur", type: ExchangeType.Topic, durable: true);
    }

    public void SendMessage<T>(T message, string routingKey)
    {
        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        _channel.BasicPublish(
            exchange: "ms.utilisateur",
            routingKey: routingKey,
            basicProperties: null,
            body: body
        );
    }
}