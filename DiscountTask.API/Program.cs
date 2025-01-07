using DiscountTask.API.Data;
using DiscountTask.API.Hubs;
using DiscountTask.API.Services;
using DiscountTask.Services.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DiscountTask.API
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddControllersWithViews();
			builder.Services.AddRazorPages();

			builder.Services.AddDbContext<DiscountDbContext>(options =>
			{
				options.UseSqlite("Data Source=DiscountCodes.db");
			});

			builder.Services.AddDiscountService<StorageRepository>();

			builder.Services.AddSignalR();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseWebAssemblyDebugging();
			}
			app.UseBlazorFrameworkFiles();
			app.UseStaticFiles();

			app.UseRouting();

			app.MapRazorPages();
			app.MapControllers();
			app.MapFallbackToFile("index.html");

			app.MapHub<DiscountHub>("/discountHub");

			ApplyMigration(app);

			await app.RunAsync();
		}

		[Obsolete("do not use this method in production")]
		private static void ApplyMigration(WebApplication app)
		{
			using var scope = app.Services.CreateScope();

			scope.ServiceProvider.GetRequiredService<DiscountDbContext>().Database.Migrate();
		}
	}
}
