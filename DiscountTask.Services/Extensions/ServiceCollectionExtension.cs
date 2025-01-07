using DiscountTask.Services.Implementations;
using DiscountTask.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DiscountTask.Services.Extensions
{
	public static class ServiceCollectionExtension
	{
		public static IServiceCollection AddDiscountService<TStore>(this IServiceCollection services) where TStore : class, IStorageRepository
		{
			services.AddScoped<IStorageRepository, TStore>();
			services.AddScoped<IDiscountService, DiscountService>();

			return services;
		}
	}
}
