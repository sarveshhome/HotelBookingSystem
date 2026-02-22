namespace Common.Infrastructure;

public interface IStateStore
{
    Task<T> GetStateAsync<T>(string key);
    Task SaveStateAsync<T>(string key, T value);
    Task DeleteStateAsync(string key);
}
