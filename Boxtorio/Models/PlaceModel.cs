using Boxtorio.Data.Entities;

namespace Boxtorio.Models;

public class PlaceModel
{
	public Guid Id { get; set; }
	public Guid DeliveryPointId { get; set; }
	public int SectionId { get; set; }
	public int RackId { get; set; }
	public int ShelfId { get; set; }

	public virtual ICollection<Box>? Boxes { get; set; }
	public virtual DeliveryPoint DeliveryPoint { get; set; } = null!;
}