namespace Boxtorio.Data.Entities;

public sealed class AccountSession
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid RefreshToken { get; set; }
    public DateTimeOffset Created { get; set; }
    public bool IsActive { get; set; } = true;
    public Account? User { get; set; }
}
