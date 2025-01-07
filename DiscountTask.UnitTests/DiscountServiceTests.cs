using Microsoft.Extensions.DependencyInjection;
using Xunit;
using DiscountTask.Services.Extensions;
using DiscountTask.Services.Interfaces;
using DiscountTask.Services.Models;
using DiscountTask.UnitTests.Mocks;

namespace DiscountTask.UnitTests
{
	public class DiscountServiceTests
	{
		public ServiceProvider ServiceProvider { get; }

		public DiscountServiceTests()
		{
			var services = new ServiceCollection();

			services.AddDiscountService<StorageRepositoryMock>();

			ServiceProvider = services.BuildServiceProvider();
		}

		[Theory]
		[MemberData(nameof(DiscountCodeData))]
		public async Task GenerateDiscountCodesAsync(GenerateRequest request)
		{
			var service = ServiceProvider.GetService<IDiscountService>();

			var storage = ServiceProvider.GetService<IStorageRepository>() as StorageRepositoryMock;

			var response = await service.GenerateDiscountCodesAsync(request);

			Assert.True(response.Result);
			Assert.Equal(request.Count, storage.DiscountCodes.Count);
			Assert.True(storage.DiscountCodes.Values.All(a => a.Code.Length == request.Length));
		}

		[Theory]
		[MemberData(nameof(DiscountCodeData))]
		public async Task GetDiscountCodesAsync(GenerateRequest request)
		{
			var service = ServiceProvider.GetService<IDiscountService>();

			var storage = ServiceProvider.GetService<IStorageRepository>() as StorageRepositoryMock;

			var response = await service.GenerateDiscountCodesAsync(request);

			Assert.True(response.Result);
			Assert.Equal(request.Count, storage.DiscountCodes.Count);
			Assert.True(storage.DiscountCodes.Values.All(a => a.Code.Length == request.Length));

			var readResponse = await service.GetDiscountCodesAsync();

			Assert.NotNull(readResponse);

			var list = readResponse.ToList();

			Assert.Equal(request.Count, list.Count);
		}

		[Theory]
		[MemberData(nameof(DiscountCodeData))]
		public async Task GetDiscountCodesByCodesAsync(GenerateRequest request)
		{
			var service = ServiceProvider.GetService<IDiscountService>();

			var storage = ServiceProvider.GetService<IStorageRepository>() as StorageRepositoryMock;

			var response = await service.GenerateDiscountCodesAsync(request);

			Assert.True(response.Result);
			Assert.Equal(request.Count, storage.DiscountCodes.Count);
			Assert.True(storage.DiscountCodes.Values.All(a => a.Code.Length == request.Length));

			var codes = storage.DiscountCodes.Values.Select(a => a.Code).Take(request.Count / 2 + 1).ToList();

			var readResponse = await service.GetDiscountCodesAsync(codes);

			Assert.NotNull(readResponse);

			var list = readResponse.ToList();

			Assert.Equal(codes.Count, list.Count);
		}

		[Theory]
		[MemberData(nameof(DiscountCodeData))]
		public async Task UseDiscountCodesAsync(GenerateRequest request)
		{
			var service = ServiceProvider.GetService<IDiscountService>();

			var storage = ServiceProvider.GetService<IStorageRepository>() as StorageRepositoryMock;

			var response = await service.GenerateDiscountCodesAsync(request);

			Assert.True(response.Result);
			Assert.Equal(request.Count, storage.DiscountCodes.Count);
			Assert.True(storage.DiscountCodes.Values.All(a => a.Code.Length == request.Length));

			var readResponse = await service.GetDiscountCodesAsync();

			Assert.NotNull(readResponse);

			var list = readResponse.ToList();

			Assert.Equal(request.Count, list.Count);

			var modifed = new List<string>();

			for (var i = 0; i < list.Count; i += 2)
			{
				modifed.Add(list[i].Code);
				var useResponse = await service.UseDiscountCodeAsync(new UseCodeRequest { Code = list[i].Code });

				Assert.True(useResponse.Result);
			}

			var readResponse2 = await service.GetDiscountCodesAsync(modifed);

			Assert.NotNull(readResponse);
			Assert.True(readResponse2.All(a => a.IsUsed));
		}

		public static IEnumerable<object[]> DiscountCodeData =>
			[
				[   new GenerateRequest{ Count = 1, Length = 8 } ],
				[   new GenerateRequest{ Count = 5, Length = 8 } ],
				[   new GenerateRequest { Count = 10, Length = 8 } ],
				[   new GenerateRequest { Count = 100, Length = 8 } ],
				[   new GenerateRequest { Count = 1000, Length = 8 } ],
				[   new GenerateRequest { Count = 10000, Length = 8 } ],
				[   new GenerateRequest { Count = 3, Length = 3 } ],
				[   new GenerateRequest { Count = 5, Length = 5 } ],
				[   new GenerateRequest { Count = 10, Length = 10 } ]
			];
	}
}