namespace Film.Events;

public class EventPublisher : IEventPublisher
{
    private readonly IRabbitMQProducer _rabbitMQProducer;

    public EventPublisher(IRabbitMQProducer rabbitMQProducer)
    {
        _rabbitMQProducer = rabbitMQProducer;
    }

    public void PublishFilmEvent(FilmEvent filmEvent)
    {
        Console.WriteLine($"[RabbitMQ] Publication de l'événement film : {filmEvent.Message}");
        _rabbitMQProducer.SendMessage(filmEvent, "film.event");
    }
}