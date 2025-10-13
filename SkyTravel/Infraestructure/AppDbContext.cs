using Microsoft.EntityFrameworkCore;
using SkyTravel.Models;

namespace SkyTravel.Infraestructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    
    public DbSet<User> Users { get; set; }
    public DbSet<Flight> Flights { get; set; }
    public DbSet<ReservFlight> ReservFlights { get; set; }
}