namespace Boxtorio.Data.Entities
{
	public class DeliveryPoint
	{
		public Guid Id { get; set; }
		public string Address { get; set; } = null!;

		public virtual ICollection<Worker>? Workers { get; set; }
		public virtual ICollection<Box> Boxes { get; set; } = new List<Box>();
		public virtual ICollection<Place> Places { get; set; } = new List<Place>();
	}
}
