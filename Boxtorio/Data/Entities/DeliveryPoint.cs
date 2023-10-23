namespace Boxtorio.Data.Entities;

public sealed class DeliveryPoint
{
	public Guid Id { get; set; }
	public string Address { get; set; } = null!;

	public ICollection<Worker>? Workers { get; set; }
	public ICollection<Box>? Boxes { get; set; }
	public ICollection<Place>? Places { get; set; }
}
