using DiscountTask.Services.Models;

namespace DiscountTask.Services.Interfaces
{
	public interface IDiscountService
	{
		Task<IEnumerable<DiscountCodeModel>> GetDiscountCodesAsync(IEnumerable<string> codes = null, CancellationToken cancellationToken = default);

		Task<GenerateResponse> GenerateDiscountCodesAsync(GenerateRequest request, CancellationToken cancellationToken = default);

		Task<UseCodeResponse> UseDiscountCodeAsync(UseCodeRequest request, CancellationToken cancellationToken = default);
	}
}
