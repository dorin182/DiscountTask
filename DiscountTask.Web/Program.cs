using DiscountTask.Services.Interfaces;
using DiscountTask.Web.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace DiscountTask.Web
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("#app");
			builder.RootComponents.Add<HeadOutlet>("head::after");

			builder.Services.AddScoped<IDiscountService>(opt => new SignalRDiscountService(new Uri(new Uri(builder.HostEnvironment.BaseAddress), "/discountHub")));

			await builder.Build().RunAsync();
		}
	}
}
