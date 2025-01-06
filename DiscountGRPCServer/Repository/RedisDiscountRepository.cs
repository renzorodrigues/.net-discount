using DiscountGRPCServer.Domain.Enums;
using StackExchange.Redis;

namespace DiscountGRPCServer.Repository
{
    public class RedisDiscountRepository(IDatabase redisDb) : IDiscountRepository
    {
        private readonly IDatabase _redisDb = redisDb ?? throw new ArgumentNullException(nameof(redisDb));

        public bool TryAddCode(string code)
        {
            var result = (int)_redisDb.ScriptEvaluate(RedisScripts.AddCode, values: [code]);
            
            return result == 1;
        }

        public CodeUsageResult UseCode(string code)
        {
            var result = (int)_redisDb.ScriptEvaluate(RedisScripts.UseCode, values: [code]);
            
            return (CodeUsageResult)result;
        }
    }
}
