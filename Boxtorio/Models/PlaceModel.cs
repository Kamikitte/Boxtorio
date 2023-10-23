using Boxtorio.Data.Entities;

namespace Boxtorio.Models;

public sealed class PlaceModel
{
	public Guid Id { get; set; }
	public Guid DeliveryPointId { get; set; }
	public int SectionId { get; set; }
	public int RackId { get; set; }
	public int ShelfId { get; set; }

	public ICollection<Box>? Boxes { get; set; }
	public DeliveryPoint DeliveryPoint { get; set; } = null!;
}
