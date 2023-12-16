using Newtonsoft.Json;
using StackExchange.Redis;

namespace org.huage.BizManagement.Redis;

//实现缓存接口
public class RedisHelper : IRedisHelper
{
    private int _defaultTimeout = 600; //默认超时时间（单位秒）
    private readonly ConnectionMultiplexer _connectionMultiplexer;
    private readonly IDatabase _database;
    
    public RedisHelper(IConnectionMultiplexer redis)
    {
        _connectionMultiplexer = (ConnectionMultiplexer)redis;
        _database = _connectionMultiplexer.GetDatabase(0);
    }

    /// <summary>
    /// 删除hash 的某一个filed.
    /// </summary>
    /// <param name="redisKey"></param>
    /// <param name="filed"></param>
    /// <returns></returns>
    public async Task<bool> HashDeleteFiled(string redisKey, string filed)
    {
         return await _database.HashDeleteAsync(redisKey,filed);
    }

    /// <summary>
    /// 删除redis 的某一个key.
    /// </summary>
    /// <param name="redisKey"></param>
    /// <returns></returns>
    public async Task<bool> DelKey(string redisKey)
    {
        _database.Ping();         
        return await _database.KeyDeleteAsync(redisKey);
    }

    /// <summary>
    /// 连接超时设置
    /// </summary>
    public int TimeOut
    {
        get { return _defaultTimeout; }
        set { _defaultTimeout = value; }
    }
    
    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="key"></param>
    public void Remove(string key)
    {
        _database.KeyDelete(key, CommandFlags.HighPriority);
    }

    /// <summary>
    /// 判断key是否存在
    /// </summary>
    public bool Exists(string key)
    {
        return _database.KeyExists(key);
    }

    /// <summary>
    /// 获取指定redisKey的所有key
    /// </summary>
    /// <param name="redisKey"></param>
    /// <returns></returns>
    public async Task<List<string>> HGetAllKeys(string redisKey)
    {
        var keys =await _database.HashKeysAsync(redisKey);
        var result = new List<string>();
        foreach (var key in keys)
        {
            result.Add(JsonConvert.DeserializeObject<string>(key));
        }

        return result;
    }

    /// <summary>
    /// 批量插入
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Task<bool> HashSet(string redisKey, string hashField, string value)
    {
        return _database.HashSetAsync(redisKey, hashField, value);
    }
    
    /// <summary>
    /// 批量插入泛型
    /// </summary>
    /// <param name="redisKey"></param>
    /// <param name="hashField"></param>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Task<bool> HashSet<T>(string redisKey, string hashField, T value)
    {
        string val = JsonConvert.SerializeObject(value);
        return _database.HashSetAsync(redisKey, hashField, val);
    }
    
    /// <summary>
    /// 获取Key
    /// </summary>
    /// <param name="redisKey"></param>
    /// <returns></returns>
    public async Task<Dictionary<string,T>> HashGetAllAsync<T>(string redisKey)
    {
        var all= await _database.HashGetAllAsync(redisKey);
        Dictionary<string,T> values = new Dictionary<string,T>();
        foreach (HashEntry entry in all)
        {
            var v = entry.Value;
            T obj = JsonConvert.DeserializeObject<T>(entry.Value);
            values.Add(entry.Key,obj);
        }
        return values;
    }

    /// <summary>
    /// 获取指定redisKey的所有value
    /// </summary>
    /// <param name="redisKey"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<List<T>> HGetAllValue<T>(string redisKey)
    {
        var values =await _database.HashValuesAsync(redisKey);
        var result = new List<T>();
        foreach (var redisValue in values)
        {
            result.Add(JsonConvert.DeserializeObject<T>(redisValue));
        }

        return result;
    }


    /// <summary>
    /// Hash判断某个key是否存在filed.
    /// </summary>
    /// <param name="redisKey"></param>
    /// <param name="hashField"></param>
    /// <returns></returns>
    public Task<bool> HashExists(string redisKey, string hashField)
    {
        return _database.HashExistsAsync(redisKey, hashField);
    }
    
}