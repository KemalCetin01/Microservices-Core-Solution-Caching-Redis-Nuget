using StackExchange.Redis;

namespace MS.Services.Core.Caching.Redis.Helper;

public interface IRedisServer
{
    public IDatabase Database { get; }
}