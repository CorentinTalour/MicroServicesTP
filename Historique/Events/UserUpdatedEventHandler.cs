using Historique.Data;
using Historique.Models;
using Steeltoe.Messaging.RabbitMQ.Attributes;

namespace Historique.Events;

public class UserUpdatedEventHandler
{
    private readonly IServiceScopeFactory _scopeFactory;

    public UserUpdatedEventHandler(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
        Console.WriteLine("📢 Consumer UserUpdatedEventHandler initialisé !");
    }

    [DeclareQueue(Name = "user.updated.queue")]
    [DeclareQueueBinding(
        Name = "UserUpdatedBinding",
        QueueName = "user.updated.queue",
        ExchangeName = "ms.utilisateur",
        RoutingKey = "user.updated")]
    [RabbitListener(Binding = "UserUpdatedBinding")]
    public void HandleUserUpdated(UserUpdatedEvent message)
    {
        Console.WriteLine($"✅ Utilisateur modifié : {message.Login}");

        using (var scope = _scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<HistoriqueDbContext>();

            var log = new Log
            {
                Id = Guid.NewGuid(),
                Message = $"Utilisateur modifié : {message.Login}",
                Source = "historique-service",
                IpPort = "historique:8081",
                Code = "USER_UPDATED"
            };

            dbContext.Logs.Add(log);
            dbContext.SaveChanges();
        }

        Console.WriteLine($"✅ Log inséré pour modification : {message.Login}");
    }
}