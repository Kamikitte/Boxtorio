﻿namespace Boxtorio.Models;

public sealed class CreatePlaceModel
{
    public Guid DeliveryPointId { get; set; }
    public int SectionId { get; set; }
    public int RackId { get; set; }
    public int ShelfId { get; set; }
}
