using DiscountTask.Services.Interfaces;
using DiscountTask.Services.Models;

namespace DiscountTask.UnitTests.Mocks
{
	internal class StorageRepositoryMock : IStorageRepository
	{
		private readonly object _locker = new object();

		public Dictionary<string, DiscountCodeModel> DiscountCodes { get; } = new Dictionary<string, DiscountCodeModel>();

		Task<IEnumerable<DiscountCodeModel>> IStorageRepository.GetDiscountCodesAsync(IEnumerable<string> codes, CancellationToken cancellationToken)
			=> Task.Run(() =>
			{
				if (codes == null)
					return DiscountCodes.Values;

				var list = codes.ToList();

				return DiscountCodes.Where(code => list.Contains(code.Key)).Select(s => s.Value);
			}, cancellationToken);

		public Task<bool> StoreDiscountCodesAsync(IEnumerable<DiscountCodeModel> codes, CancellationToken cancellationToken = default)
		{
			if (codes == null)
				return Task.FromResult(false);

			cancellationToken.ThrowIfCancellationRequested();

			var list = codes.ToList();

			lock (_locker)
			{
				cancellationToken.ThrowIfCancellationRequested();

				if (list.Any(code => DiscountCodes.ContainsKey(code.Code)))
					return Task.FromResult(false);

				foreach (var code in list)
					DiscountCodes.Add(code.Code, code);
			}

			return Task.FromResult(true);
		}

		public Task<bool> UseDiscountCodeAsync(string code, CancellationToken cancellationToken = default)
		{
			if (!DiscountCodes.TryGetValue(code, out var obj))
				return Task.FromResult(false);

			if (obj.IsUsed)
				return Task.FromResult(false);

			lock (obj)
			{
				if (obj.IsUsed)
					return Task.FromResult(false);

				cancellationToken.ThrowIfCancellationRequested();

				obj.IsUsed = true;
			}
			return Task.FromResult(true);
		}
	}
}
