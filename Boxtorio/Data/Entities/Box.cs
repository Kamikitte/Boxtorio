namespace Boxtorio.Data.Entities
{
	public class Box
	{
		public Guid Id { get; set; }
		public Guid DeliveryPointId { get; set; }
		public Guid PlaceId { get; set; }
		public int CustomerId { get; set; }
		public int OrderId { get; set; }
		public int PackageId { get; set; }
		public virtual DeliveryPoint DeliveryPoint { get; set; } = null!;
		public virtual Place? Place { get; set;}
	}
}
