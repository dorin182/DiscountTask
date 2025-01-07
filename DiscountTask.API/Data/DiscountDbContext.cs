using Microsoft.EntityFrameworkCore;

namespace DiscountTask.API.Data
{
	public class DiscountDbContext : DbContext
	{
		public DiscountDbContext(DbContextOptions<DiscountDbContext> options) : base(options)
		{
		}

		public DbSet<DiscountCode> DiscountCodes { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<DiscountCode>(b =>
			{
				b
				.HasIndex(p => p.Code)
				.IsUnique(true);
			});
		}
	}
}
