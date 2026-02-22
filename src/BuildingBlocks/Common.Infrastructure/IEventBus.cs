namespace Common.Infrastructure;

public interface IEventBus
{
    Task PublishAsync<T>(string topicName, T eventData) where T : class;
}
