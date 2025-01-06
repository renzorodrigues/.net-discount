using DiscountGRPCServer.Domain.Enums;

namespace DiscountGRPCServer.Repository
{
    public interface IDiscountRepository
    {
        bool TryAddCode(string code);
        CodeUsageResult UseCode(string code);
    }
}
