using System.Text;
using Historique.Data;
using Historique.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Steeltoe.Messaging.RabbitMQ.Attributes;

namespace Historique.Events;

public class DetailFilmEventHandler
{
    private readonly IServiceScopeFactory _scopeFactory;

    public DetailFilmEventHandler(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
        Console.WriteLine("Handler DetailFilmEventHandler initialisé !");
    }

    [DeclareQueue(Name = "detailfilm.queue")]
    [DeclareQueueBinding(
        Name = "DetailFilmEventBinding",
        QueueName = "detailfilm.queue",
        ExchangeName = "ms.detailfilm",
        RoutingKey = "detailfilm.event")]
    [RabbitListener(Binding = "DetailFilmEventBinding")]
    public void HandleDetailFilmEvent(DetailFilmEvent message)    
    {
        Console.WriteLine($"Event reçu - {message.Message}");

        using var scope = _scopeFactory.CreateScope();
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

        Console.WriteLine($"Log inséré : {message.Message}");
    }}