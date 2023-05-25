namespace Boxtorio.Data.Entities
{
	public class Account
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string PasswordHash { get; set; } = null!;
		public string Role { get; set; } = null!;

		public virtual ICollection<AccountSession>? Sessions { get; set; }
	}
}