namespace Boxtorio.Models;

public class CreateBoxModel
{
	public Guid DeliveryPointId { get; set; }
	public Guid PlaceId { get; set; }
	public int CustomerId { get; set; }
	public int OrderId { get; set; }
	public int PackageId { get; set; }
}