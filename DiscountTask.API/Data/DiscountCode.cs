using System.ComponentModel.DataAnnotations;

namespace DiscountTask.API.Data
{
	public class DiscountCode
	{
		[Key]
		public Guid Guid { get; set; }

		public string Code { get; set; }

		public bool IsUsed { get; set; }

		[Timestamp]
		public byte[] RowVersion { get; set; }
	}
}
