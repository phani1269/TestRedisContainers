using StackExchange.Redis;

namespace CacheLibrary.Repositories
{
    public interface ICacheService
    {
        T GetData<T>(string key, int db);
        T GetString<T>(string key);
        bool SetData<T>(string key, T value, int expirationTime, int db);
        bool SetData<T>(string key, T value, int db);
        bool CheckKeyExist(string key);
        bool SetAdd<T>(string key, T value);
        IEnumerable<T> GetSetData<T>(string Key);
        bool RemoveSetData<T>(string Key, T Value);
        bool RemoveKey(string Key);
        object RemoveData(string key);
        List<RedisValue> GetHashKeys(string Key);
        IEnumerable<T> GetAllHashData<T>(string Key);
        T GetHashFieldData<T>(string Key);
    }
}
