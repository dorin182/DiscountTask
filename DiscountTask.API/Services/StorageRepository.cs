using Microsoft.EntityFrameworkCore;
using DiscountTask.Services.Interfaces;
using DiscountTask.Services.Models;
using DiscountTask.API.Data;

namespace DiscountTask.API.Services
{
	internal class StorageRepository : IStorageRepository
	{
		private readonly DiscountDbContext _context;

		public StorageRepository(DiscountDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<DiscountCodeModel>> GetDiscountCodesAsync(IEnumerable<string> codes = null, CancellationToken cancellationToken = default)
		{
			var query = _context.DiscountCodes.AsQueryable();

			if (codes != null)
			{
				var list = codes.ToList();
				if (list.Count > 0)
					query = query.Where(w => list.Contains(w.Code));
			}

			return await query
							.Select(s => new DiscountCodeModel
							{
								Code = s.Code,
								IsUsed = s.IsUsed
							})
							.ToListAsync(cancellationToken);
		}

		public async Task<bool> StoreDiscountCodesAsync(IEnumerable<DiscountCodeModel> codes, CancellationToken cancellationToken = default)
		{
			using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				await _context.DiscountCodes.AddRangeAsync(codes.Select(s => new DiscountCode
				{
					Code = s.Code,
					IsUsed = s.IsUsed
				}), cancellationToken);

				var result = await _context.SaveChangesAsync(cancellationToken);

				await transaction.CommitAsync(cancellationToken);

				return result > 0;
			}
			catch (Exception)
			{
				await transaction.RollbackAsync();
				return false;
			}
		}

		public async Task<bool> UseDiscountCodeAsync(string code, CancellationToken cancellationToken = default)
		{
			try
			{
				var obj = await _context.DiscountCodes.FirstOrDefaultAsync(f => f.Code == code, cancellationToken);

				if (obj == null)
					return false;

				if (obj.IsUsed)
					return false;

				obj.IsUsed = true;

				var result = await _context.SaveChangesAsync(cancellationToken);

				return result > 0;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
