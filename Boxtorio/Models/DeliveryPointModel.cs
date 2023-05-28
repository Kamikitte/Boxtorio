using Boxtorio.Data.Entities;

namespace Boxtorio.Models
{
	public class DeliveryPointModel
	{
		public Guid Id { get; set; }
		public string Address { get; set; } = null!;

		public virtual ICollection<Worker>? Workers { get; set; }
		public virtual ICollection<Box>? Boxes { get; set; }
		public virtual ICollection<Place>? Places { get; set; }
	}
}
