using Dapr.Client;

namespace Common.Infrastructure;

public class DaprStateStore : IStateStore
{
    private readonly DaprClient _daprClient;
    private const string StoreName = "statestore";

    public DaprStateStore(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    public async Task<T> GetStateAsync<T>(string key)
    {
        return await _daprClient.GetStateAsync<T>(StoreName, key);
    }

    public async Task SaveStateAsync<T>(string key, T value)
    {
        await _daprClient.SaveStateAsync(StoreName, key, value);
    }

    public async Task DeleteStateAsync(string key)
    {
        await _daprClient.DeleteStateAsync(StoreName, key);
    }
}
