using DiscountGRPC;
using DiscountGRPCServer.Domain;
using DiscountGRPCServer.Domain.Enums;
using Grpc.Core;

namespace DiscountGRPCServer.Services
{
    public class DiscountServiceGrpc(IDiscountManager discountManager) : DiscountService.DiscountServiceBase
    {
        private readonly IDiscountManager _discountManager = discountManager;

        public override Task<GenerateCodesResponse> GenerateCodes(
            GenerateCodesRequest request, ServerCallContext context)
        {
            var response = new GenerateCodesResponse();
            
            try
            {
                var codes = _discountManager.GenerateCodes((int)request.Count, (int)request.Length);
                response.Result = true;
                response.Codes.AddRange(codes);
            }
            catch (System.Exception ex)
            {
                response.Result = false;
                response.ErrorMessage = ex.Message;
            }

            return Task.FromResult(response);
        }

        public override Task<UseCodeResponse> UseCode(
            UseCodeRequest request, ServerCallContext context)
        {
            var result = (uint)_discountManager.UseCode(request.Code);

            var response = new UseCodeResponse
            {
                Result = result,
                ErrorMessage = result switch
                {
                    (uint)CodeUsageResult.NotFound => "Code not found.",
                    (uint)CodeUsageResult.AlreadyUsed => "Code already used.",
                    (uint)CodeUsageResult.Invalid => "Code is invalid.",
                    _ => string.Empty
                }
            };

            return Task.FromResult(response);
        }
    }
}
