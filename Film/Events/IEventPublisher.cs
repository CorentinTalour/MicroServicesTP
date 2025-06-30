namespace Film.Events;

public interface IEventPublisher
{
    void PublishFilmEvent(FilmEvent filmEvent);
}