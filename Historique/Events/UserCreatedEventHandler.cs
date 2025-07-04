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
        Console.WriteLine("Handler UserCreatedEventHandler initialisé !");
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
        Console.WriteLine($"Event reçu - login : {message.Login}");

        using (var scope = _scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<HistoriqueDbContext>();

            var log = new Log
            {
                Id = Guid.NewGuid(),
                Message = $"Nouvel utilisateur créé : {message.Login}",
                Source = "historique-service",
                IpPort = "historique:8081",
                Code = "USER_CREATED"
            };

            dbContext.Logs.Add(log);
            dbContext.SaveChanges();
        }

        Console.WriteLine($"Log inséré pour : {message.Login}");
    }
}