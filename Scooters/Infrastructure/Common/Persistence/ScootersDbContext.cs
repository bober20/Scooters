using System.Reflection;

namespace Infrastructure.Common.Persistence;

public class ScootersDbContext : DbContext, IUnitOfWork
{
    private const string _dbName = "scooters.db";
    private string _dbPath;
    
    public DbSet<Scooter> Scooters { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Ride> Rides { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    
    public ScootersDbContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        _dbPath = System.IO.Path.Combine(path, _dbName);
        
        Database.EnsureCreated();
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={_dbPath}");
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