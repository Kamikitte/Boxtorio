namespace Boxtorio.Data.Entities;

public sealed class Worker : Account
{
	public Guid? DeliveryPointId { get; set; }
	public DeliveryPoint? DeliveryPoint { get; set; }
}
