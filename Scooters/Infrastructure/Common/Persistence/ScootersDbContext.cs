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
        // Database.EnsureDeleted();
        Database.EnsureCreated();
        InitializeDatabase();
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
    
    public void InitializeDatabase()
    {
        if (!Scooters.Any())
        {
            var scooters = new List<Scooter>
            {
                new Scooter
                {
                    Id = Guid.NewGuid(),
                    ModelDescription = "Standard Scooter",
                    Coordinates = new Coordinates { Latitude = 52.237049, Longitude = 21.017532 },
                    Reservations = new List<Reservation>(),
                    Rides = new List<Ride>()
                },
                new Scooter
                {
                    Id = Guid.NewGuid(),
                    ModelDescription = "Premium Scooter",
                    Coordinates = new Coordinates { Latitude = 52.232180, Longitude = 21.006100 },
                    Reservations = new List<Reservation>(),
                    Rides = new List<Ride>()
                },
                new Scooter
                {
                    Id = Guid.NewGuid(),
                    ModelDescription = "Urban Explorer",
                    Coordinates = new Coordinates { Latitude = 52.239750, Longitude = 21.026320 },
                    Reservations = new List<Reservation>(),
                    Rides = new List<Ride>()
                }
            };

            Scooters.AddRange(scooters);
            SaveChanges();
        }
    }
}