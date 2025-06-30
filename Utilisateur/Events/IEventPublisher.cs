namespace Utilisateur.Events;

public interface IEventPublisher
{
    void PublishUserCreated(UserCreatedEvent userCreatedEvent);
    void PublishUserUpdated(UserUpdatedEvent userUpdatedEvent);
    void PublishUserDeleted(UserDeletedEvent userDeletedEvent);
    void PublishUserAccessed(UserAccessedEvent userAccessedEvent);

}