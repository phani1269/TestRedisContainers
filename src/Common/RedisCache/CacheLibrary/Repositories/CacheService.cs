using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text;
using System.Text.Json;

namespace CacheLibrary.Repositories
{
    public class CacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDistributedCache _distributedCache;

        public CacheService(IConnectionMultiplexer connectionMultiplexer, IDistributedCache distributedCache)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _distributedCache = distributedCache;
        }

        public T GetString<T>(string key)
        {
            var data = _distributedCache.GetString(key);
           return JsonSerializer.Deserialize<T>(data);

        }
        
        public T GetData<T>(string key, int db)
        {
            var _db = _connectionMultiplexer.GetDatabase(db);
            var value = _db.StringGet(key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonSerializer.Deserialize<T>(value);
            }
            return default;
        }

        public object RemoveData(string key)
        {
            var _db = _connectionMultiplexer.GetDatabase(0);
            var value = _db.KeyExists(key);
            if (value)
            {
                return _db.KeyDelete(key);
            }
            return false;
        }

        public bool SetData<T>(string key, T value, int Time, int dbposition)
        {
            var db = _connectionMultiplexer.GetDatabase(dbposition);
            var serial = JsonSerializer.Serialize(value);
            var expiryTime = DateTimeOffset.Now.AddMinutes(Time);
            var expiry = expiryTime.DateTime.Subtract(DateTime.Now);
            var set = db.StringSet(key, serial, expiry);
            return set;
        }

        public bool SetData<T>(string key, T value, int dbposition)
        {
            var db = _connectionMultiplexer.GetDatabase(dbposition);
            var serial = JsonSerializer.Serialize(value);
            var set = db.StringSet(key, serial);
            
            return set;
        }

       public bool CheckKeyExist(string key)
        {
            var db = _connectionMultiplexer.GetDatabase(1);
           return db.KeyExists(key);
        }
        public bool SetAdd<T>(string key , T value)
        {
            var db = _connectionMultiplexer.GetDatabase(1);
            var serial = JsonSerializer.Serialize(value);

           return  db.SetAdd(key, serial);

        }

        public IEnumerable<T> GetSetData<T>(string Key)
        {
            var db = _connectionMultiplexer.GetDatabase(1);
            var completeSet = db.SetMembers(Key);
            if (completeSet.Length>0)
            {
              var obj=  Array.ConvertAll(completeSet, val => JsonSerializer.Deserialize<T>(val)).ToList();
                return obj;
            }
            return null;
        }

        public bool RemoveSetData<T>(string Key, T Value)
        {
            var db = _connectionMultiplexer.GetDatabase(1);
            var serial = JsonSerializer.Serialize(Value);
            return  db.SetRemove(Key, serial);

        }

        public bool RemoveKey(string Key)
        {
            var db = _connectionMultiplexer.GetDatabase(1);
            
           return db.KeyDelete(Key);
        }
        public List<RedisValue> GetHashKeys(string Key)
        {
            var db = _connectionMultiplexer.GetDatabase(0);
            var keys = db.HashKeys(Key).ToList();
            return keys;
        }

        public T GetHashFieldData<T>(string Key)
        {
           var  field = "data";
            var db = _connectionMultiplexer.GetDatabase(0);
            var hashset = db.HashGet(Key, field);

           var str = Encoding.ASCII.GetString(hashset);
           var arr= str.Split("?").Last();
            
            
            return JsonSerializer.Deserialize<T>(arr);
            // var serial = Encoding.UTF8.GetString(arr);
        }
        public IEnumerable<T> GetAllHashData<T>(string Key)
        {
            var db = _connectionMultiplexer.GetDatabase(0);
            var hashset = db.HashGetAll(Key);
            if (hashset.Length > 0)
            {
                var obj = Array.ConvertAll(hashset, val => JsonSerializer.Deserialize<T>(val.Value)).ToList();
                return obj;
            }
            return null;
        }
       



    }
}
