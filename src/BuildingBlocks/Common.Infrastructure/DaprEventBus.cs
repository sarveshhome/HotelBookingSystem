using Dapr.Client;

namespace Common.Infrastructure;

public class DaprEventBus : IEventBus
{
    private readonly DaprClient _daprClient;
    private const string PubSubName = "hotelbooking-pubsub";

    public DaprEventBus(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    public async Task PublishAsync<T>(string topicName, T eventData) where T : class
    {
        await _daprClient.PublishEventAsync(PubSubName, topicName, eventData);
    }
}
