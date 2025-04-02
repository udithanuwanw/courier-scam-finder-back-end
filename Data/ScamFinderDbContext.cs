namespace courier_scam_finder_back_end.Data;
using courier_scam_finder_back_end.Models;
using Microsoft.EntityFrameworkCore;

public class ScamFinderDbContext : DbContext
{
    public ScamFinderDbContext(DbContextOptions<ScamFinderDbContext> options) : base(options) { }
    public DbSet<User> Users { get; set; }
    public DbSet<Scammer> Scammers { get; set; }
}
