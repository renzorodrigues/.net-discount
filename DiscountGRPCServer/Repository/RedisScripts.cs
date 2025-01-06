namespace DiscountGRPCServer.Repository
{
    public static class RedisScripts
    {
        public const string AddCode = @"
            local code = ARGV[1]
            if redis.call('SISMEMBER', 'unusedCodes', code) == 1 or 
               redis.call('SISMEMBER', 'usedCodes', code) == 1 then
                return 0
            else
                redis.call('SADD', 'unusedCodes', code)
                return 1
            end";

        public const string UseCode = @"
            local code = ARGV[1]
            if redis.call('SISMEMBER', 'usedCodes', code) == 1 then
                return 2
            elseif redis.call('SISMEMBER', 'unusedCodes', code) == 1 then
                redis.call('SREM', 'unusedCodes', code)
                redis.call('SADD', 'usedCodes', code)
                return 0
            else
                return 1
            end";
    }
}
