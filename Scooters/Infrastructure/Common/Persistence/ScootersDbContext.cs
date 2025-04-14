using System.Reflection;
using Infrastructure.CompiledModels;

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
        optionsBuilder.UseModel(ScootersDbContextModel.Instance);
        optionsBuilder.UseSqlite($"Data Source={_dbPath}");
        base.OnConfiguring(optionsBuilder);
    }
    
    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    //     base.OnModelCreating(modelBuilder);
    // }
    
    public async Task SaveChangesAsync()
    {
        await base.SaveChangesAsync();
    }
    
    private void InitializeDatabase()
    {
        if (!Scooters.Any())
        {
            var scooters = new List<Scooter>
            {
                new Scooter
                {
                    Id = Guid.NewGuid(),
                    ModelDescription = "Standard Scooter",
                    Coordinates = new Coordinates { Latitude = 53.905043, Longitude = 27.557062 },
                    Reservations = new List<Reservation>(),
                    Rides = new List<Ride>()
                },
                new Scooter
                {
                    Id = Guid.NewGuid(),
                    ModelDescription = "Premium Scooter",
                    Coordinates = new Coordinates { Latitude = 53.903362, Longitude = 27.556709 },
                    Reservations = new List<Reservation>(),
                    Rides = new List<Ride>()
                },
                new Scooter
                {
                    Id = Guid.NewGuid(),
                    ModelDescription = "Urban Explorer",
                    Coordinates = new Coordinates { Latitude = 53.905646, Longitude = 27.561266 },
                    Reservations = new List<Reservation>(),
                    Rides = new List<Ride>()
                }
            };

            Scooters.AddRange(scooters);
            SaveChanges();
        }
    }
}