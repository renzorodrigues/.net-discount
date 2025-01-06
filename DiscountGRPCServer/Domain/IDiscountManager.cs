using DiscountGRPCServer.Domain.Enums;

namespace DiscountGRPCServer.Domain
{
    public interface IDiscountManager
    {
        List<string> GenerateCodes(int count, int length);

        CodeUsageResult UseCode(string code);
    }
}
