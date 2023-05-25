namespace Boxtorio.Data.Entities
{
	public class Place
	{
		public Guid Id { get; set; }
		public Guid DeliveryPointId { get; set; }
		public int SectionId { get; set; }
		public int RackId { get; set; }
		public int ShelfId { get; set; }

		public virtual ICollection<Box>? Boxes { get; set; }
	}
}
