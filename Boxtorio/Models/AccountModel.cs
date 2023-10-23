namespace Boxtorio.Models;

public class AccountModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
}

public sealed class WorkerModel : AccountModel
{
    public Guid? DeliveryPointId { get; set; }
}
