using DiscountTask.Services.Interfaces;
using DiscountTask.Services.Models;
using Microsoft.AspNetCore.SignalR;

namespace DiscountTask.API.Hubs
{
	public class DiscountHub : Hub//, IDiscountService
	{
		private readonly IDiscountService discountService;

		public DiscountHub(IDiscountService discountService)
		{
			this.discountService = discountService;
		}

		public Task<GenerateResponse> GenerateDiscountCodesAsync(GenerateRequest request)
			=> discountService.GenerateDiscountCodesAsync(request);

		public Task<IEnumerable<DiscountCodeModel>> GetDiscountCodesAsync(IEnumerable<string> codes = null)
			=> discountService.GetDiscountCodesAsync(codes);

		public Task<UseCodeResponse> UseDiscountCodeAsync(UseCodeRequest request)
			=> discountService.UseDiscountCodeAsync(request);
	}
}
