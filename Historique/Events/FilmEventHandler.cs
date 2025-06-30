using Historique.Data;
using Historique.Models;
using Steeltoe.Messaging.RabbitMQ.Attributes;

namespace Historique.Events;

public class FilmEventHandler
{
    private readonly IServiceScopeFactory _scopeFactory;

    public FilmEventHandler(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
        Console.WriteLine("Handler FilmEventHandler initialisé !");
    }

    [DeclareQueue(Name = "film.event.queue")]
    [DeclareQueueBinding(
        Name = "FilmEventBinding",
        QueueName = "film.event.queue",
        ExchangeName = "ms.film",
        RoutingKey = "film.event")]
    [RabbitListener(Binding = "FilmEventBinding")]
    public void HandleFilmEvent(FilmEvent message)
    {
        Console.WriteLine($"Event reçu - {message.Message}");

        using (var scope = _scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<HistoriqueDbContext>();

            var log = new Log
            {
                Id = Guid.NewGuid(),
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