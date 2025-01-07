using DiscountTask.Services.Interfaces;
using DiscountTask.Services.Models;
using Microsoft.AspNetCore.SignalR.Client;

namespace DiscountTask.Web.Services
{
	public class SignalRDiscountService : IDiscountService
	{
		private HubConnection hubConnection;

		public SignalRDiscountService(Uri serverUri)
		{
			hubConnection = new HubConnectionBuilder()
				.WithUrl(serverUri)
				.WithAutomaticReconnect()
				.Build();

			hubConnection.StartAsync();
		}

		public Task<GenerateResponse> GenerateDiscountCodesAsync(GenerateRequest request, CancellationToken cancellationToken = default)
			=> hubConnection.InvokeAsync<GenerateResponse>(nameof(IDiscountService.GenerateDiscountCodesAsync), request);

		public Task<IEnumerable<DiscountCodeModel>> GetDiscountCodesAsync(IEnumerable<string> codes = null, CancellationToken cancellationToken = default)
			=> hubConnection.InvokeAsync<IEnumerable<DiscountCodeModel>>(nameof(IDiscountService.GetDiscountCodesAsync), codes);

		public Task<UseCodeResponse> UseDiscountCodeAsync(UseCodeRequest request, CancellationToken cancellationToken = default)
			=> hubConnection.InvokeAsync<UseCodeResponse>(nameof(IDiscountService.UseDiscountCodeAsync), request);
	}
}
