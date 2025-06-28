using Historique.Data;
using Historique.Models;
using Steeltoe.Messaging.RabbitMQ.Attributes;

namespace Historique.Events;

public class UserDeletedEventHandler
{
    private readonly IServiceScopeFactory _scopeFactory;

    public UserDeletedEventHandler(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
        Console.WriteLine("📢 Consumer UserDeletedEventHandler initialisé !");
    }

    [DeclareQueue(Name = "user.deleted.queue")]
    [DeclareQueueBinding(
        Name = "UserDeletedBinding",
        QueueName = "user.deleted.queue",
        ExchangeName = "ms.utilisateur",
        RoutingKey = "user.deleted")]
    [RabbitListener(Binding = "UserDeletedBinding")]
    public void HandleUserDeleted(UserDeletedEvent message)
    {
        Console.WriteLine($"✅ Utilisateur supprimé : {message.Login}");

        using (var scope = _scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<HistoriqueDbContext>();

            var log = new Log
            {
                Id = Guid.NewGuid(),
                Message = $"Utilisateur supprimé : {message.Login}",
                Source = "historique-service",
                IpPort = "historique:8081",
                Code = "USER_DELETED"
            };

            dbContext.Logs.Add(log);
            dbContext.SaveChanges();
        }

        Console.WriteLine($"✅ Log inséré pour suppression : {message.Login}");
    }
}