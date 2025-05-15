namespace MindTrack.Db;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions
        <AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<SocialAnxietyRecord> SocialAnxietyRecords { get; set; }
}