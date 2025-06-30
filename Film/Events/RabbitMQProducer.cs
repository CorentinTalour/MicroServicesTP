using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Film.Events
{
    public class RabbitMQProducer : IRabbitMQProducer
    {
        public void SendMessage<T>(T message, string routingKey)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "rabbitmq",
                UserName = "myuser",
                Password = "mypassword"
            };            
            
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            // Déclarer l’échange s’il n’existe pas encore
            channel.ExchangeDeclare(exchange: "ms.film", type: ExchangeType.Topic, durable: true);
            
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            Console.WriteLine($"[RabbitMQ] Envoi du message avec la clé {routingKey}");

            channel.BasicPublish(exchange: "ms.film",
                routingKey: routingKey,
                basicProperties: null,
                body: body);
        }
    }
}