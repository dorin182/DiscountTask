using DiscountTask.Services.Interfaces;
using DiscountTask.Services.Models;
using System.Security.Cryptography;
using System.Text;

namespace DiscountTask.Services.Implementations
{
	internal class DiscountService : IDiscountService
	{
		private readonly IStorageRepository _storageRepository;

		private const string _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ01234567890";

		public DiscountService(IStorageRepository storageRepository)
		{
			_storageRepository = storageRepository;
		}

		public async Task<GenerateResponse> GenerateDiscountCodesAsync(GenerateRequest request, CancellationToken cancellationToken = default)
		{
			var generated = new HashSet<string>();

			var random = RandomNumberGenerator.Create();

			while (generated.Count < request.Count)
			{
				while (generated.Count < request.Count)
				{
					generated.Add(GenerateDiscountCode(random, request.Length));
				}

				List<DiscountCodeModel> duplicates = (await _storageRepository.GetDiscountCodesAsync(generated, cancellationToken)).ToList();

				generated.RemoveWhere(code => duplicates.Any(a => a.Code == code));
			}

			var result = await _storageRepository.StoreDiscountCodesAsync(generated.Select(code => new DiscountCodeModel { Code = code }), cancellationToken);

			return new GenerateResponse
			{
				Result = result
			};
		}

		private static string GenerateDiscountCode(RandomNumberGenerator random, byte length)
		{
			var builder = new StringBuilder();

			var data = new byte[length];

			random.GetBytes(data);

			foreach (var bt in data)
			{
				var index = bt % _alphabet.Length;

				builder.Append(_alphabet[index]);
			}

			return builder.ToString();
		}

		public Task<IEnumerable<DiscountCodeModel>> GetDiscountCodesAsync(IEnumerable<string> codes = null, CancellationToken cancellationToken = default)
			=> _storageRepository.GetDiscountCodesAsync(codes, cancellationToken);

		public async Task<UseCodeResponse> UseDiscountCodeAsync(UseCodeRequest request, CancellationToken cancellationToken = default)
		{
			return new UseCodeResponse
			{
				Result = await _storageRepository.UseDiscountCodeAsync(request.Code, cancellationToken)
			};
		}
	}
}
