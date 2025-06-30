using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace DetailFilm.Events;

public interface IRabbitMQProducer
{
    void SendMessage<T>(T message, string exchange, string routingKey);
}

public class RabbitMQProducer : IRabbitMQProducer
{
    private readonly ConnectionFactory _factory;

    public RabbitMQProducer()
    {
        _factory = new ConnectionFactory()
        {
            HostName = "rabbitmq",
            UserName = "myuser",
            Password = "mypassword"
        };
    }

    public void SendMessage<T>(T message, string exchange, string routingKey)
    {
        using var connection = _factory.CreateConnection();
        using var channel = connection.CreateModel();

        Console.WriteLine($"[RabbitMQProducer] Publishing message to exchange '{exchange}' with routing key '{routingKey}'");
        
        channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Topic, durable: true);

        var json = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(json);

        channel.BasicPublish(exchange: exchange, routingKey: routingKey, basicProperties: null, body: body);
    }
}