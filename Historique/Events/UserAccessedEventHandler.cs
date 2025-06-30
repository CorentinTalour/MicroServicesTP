using Historique.Data;
using Historique.Models;
using Steeltoe.Messaging.RabbitMQ.Attributes;

namespace Historique.Events;

public class UserAccessedEventHandler
{
    private readonly IServiceScopeFactory _scopeFactory;

    public UserAccessedEventHandler(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
        Console.WriteLine("Handler UserAccessedEventHandler initialisé !");
    }

    [DeclareQueue(Name = "user.accessed.queue")]
    [DeclareQueueBinding(
        Name = "UserAccessedBinding",
        QueueName = "user.accessed.queue",
        ExchangeName = "ms.utilisateur",
        RoutingKey = "user.accessed")]
    [RabbitListener(Binding = "UserAccessedBinding")]
    public void HandleUserAccessed(UserAccessedEvent message)
    {
        Console.WriteLine($"Event reçu - {message.Message}");

        using (var scope = _scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<HistoriqueDbContext>();

            var log = new Log
            {
                Id = Guid.NewGuid(),
                Date = message.Date,
                Message = message.Message,
                Source = message.Source,
                IpPort = "historique:8081",
                Code = message.Code
            };

            dbContext.Logs.Add(log);
            dbContext.SaveChanges();
        }

        Console.WriteLine($"Log inséré : {message.Message}");
    }
}