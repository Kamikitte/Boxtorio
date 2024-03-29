﻿namespace Boxtorio.Models;

public sealed class BoxModel
{
    public Guid Id { get; set; }
    public Guid DeliveryPointId { get; set; }
    public Guid PlaceId { get; set; }
    public int CustomerId { get; set; }
    public int OrderId { get; set; }
    public int PackageId { get; set; }
}
