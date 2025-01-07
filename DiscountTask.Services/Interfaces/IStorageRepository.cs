using DiscountTask.Services.Models;

namespace DiscountTask.Services.Interfaces
{
	public interface IStorageRepository
	{
		Task<IEnumerable<DiscountCodeModel>> GetDiscountCodesAsync(IEnumerable<string> codes = null, CancellationToken cancellationToken = default);

		Task<bool> StoreDiscountCodesAsync(IEnumerable<DiscountCodeModel> codes, CancellationToken cancellationToken = default);

		Task<bool> UseDiscountCodeAsync(string code, CancellationToken cancellationToken = default);
	}
}
