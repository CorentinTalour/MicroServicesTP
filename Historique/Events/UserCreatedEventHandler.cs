using Historique.Data;
using Historique.Models;
using Microsoft.EntityFrameworkCore;
using Steeltoe.Messaging.RabbitMQ.Attributes;

namespace Historique.Events;

public class UserCreatedEventHandler
{
    private readonly IServiceScopeFactory _scopeFactory;

    public UserCreatedEventHandler(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    [DeclareQueue(Name = "user.created.queue")]
    [DeclareQueueBinding(
        Name = "UserCreatedBinding",
        QueueName = "user.created.queue",
        ExchangeName = "ms.utilisateur",
        RoutingKey = "user.created")]
    [RabbitListener(Binding = "UserCreatedBinding")]
    public void HandleUserCreated(UserCreatedEvent message)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<HistoriqueDbContext>();

            var log = new Log
            {
                Id = Guid.NewGuid(),
                Message = $"Nouvel utilisateur créé : {message.Login}",
                Source = "historique-service",
                IpPort = "historique:8080",
                Code = "USER_CREATED"
            };

            dbContext.Logs.Add(log);
            dbContext.SaveChanges();
        }

        Console.WriteLine($"✅ Event reçu : {message.Login}");
    }
}