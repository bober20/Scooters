using System.Reflection;

namespace Infrastructure.Common.Persistence;

public class ScootersDbContext : DbContext, IUnitOfWork
{
    public DbSet<Scooter> Scooters { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Ride> Rides { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    
    private string DbPath { get; }
    
    public ScootersDbContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Combine(path, "scooters.db");
        
        Database.EnsureCreated();
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={DbPath}");
        base.OnConfiguring(optionsBuilder);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
    
    public async Task SaveChangesAsync()
    {
        await base.SaveChangesAsync();
    }
}