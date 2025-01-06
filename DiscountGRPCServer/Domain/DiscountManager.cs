
using DiscountGRPCServer.Domain.Enums;
using DiscountGRPCServer.Repository;
using System.Text;

namespace DiscountGRPCServer.Domain
{
    public class DiscountManager(IDiscountRepository repository) : IDiscountManager
    {
        private static readonly Random _random = new();
        private readonly IDiscountRepository _repository = repository;

        public List<string> GenerateCodes(int count, int length)
        {
            if (count <= 0 || count > 2000)
                throw new ArgumentException("The number of codes must be between 1 and 2000.", nameof(count));
            
            if (length != 7 && length != 8)
                throw new ArgumentException("The code length must be 7 or 8.", nameof(length));

            var codes = new List<string>();
            
            for (int i = 0; i < count; i++)
            {
                string code;
                do
                {
                    code = GenerateRandomCode(length);
                }
                while (!_repository.TryAddCode(code));

                codes.Add(code);
            }

            return codes;
        }

        public CodeUsageResult UseCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return CodeUsageResult.Invalid;

            return _repository.UseCode(code);
        }

        private static string GenerateRandomCode(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            
            var sb = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                sb.Append(chars[_random.Next(chars.Length)]);
            }

            return sb.ToString();
        }
    }
}
