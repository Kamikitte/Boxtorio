using Boxtorio.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Boxtorio.Data
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Account>().HasIndex(f => f.Email).IsUnique();
			modelBuilder.Entity<Admin>().ToTable(nameof(Admins));
			modelBuilder.Entity<Worker>().ToTable(nameof(Workers));
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
			=> optionsBuilder.UseNpgsql(b => b.MigrationsAssembly("Boxtorio"));

		public DbSet<Account> Accounts => Set<Account>();
		public DbSet<Admin> Admins => Set<Admin>();
		public DbSet<Worker> Workers => Set<Worker>();
		public DbSet<AccountSession> AccountSessions => Set<AccountSession>();
		public DbSet<DeliveryPoint> DeliveryPoints => Set<DeliveryPoint>();
		public DbSet<Box> Boxes => Set<Box>();
		public DbSet<Place> Places => Set<Place>();
	}
}