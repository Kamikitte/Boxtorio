﻿namespace Boxtorio.Data.Entities
{
	public class Worker : Account
	{
		public Guid? DeliveryPointId { get; set; }
		public virtual DeliveryPoint? DeliveryPoint { get; set; }
	}
}
