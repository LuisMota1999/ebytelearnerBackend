using ebyteLearner.Models;
using Microsoft.EntityFrameworkCore;

public class DBContextService : DbContext
{

    public DbSet<User> User { get; set; }
    public DbSet<Course> Course { get; set; }
    public DbSet<Module> Module { get; set; }
    public DbSet<Pdf> Pdf { get; set; }
    public DbSet<Question> Question { get; set; }
    public DbSet<Answer> Answer { get; set; }
    public DbSet<Session> Session { get; set; }
    public DbSet<SessionMonitoring> SessionMonitoring { get; set; }
    public DbSet<UserSession> UserSession { get; set; }
    public DbSet<Category> Category { get; set; }
    public DBContextService(DbContextOptions<DBContextService> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserSession>()
            .HasKey(us => new { us.UserId, us.SessionId });

        modelBuilder.Entity<UserSession>()
            .HasOne(us => us.User)
            .WithMany(u => u.UserSessions)
            .HasForeignKey(us => us.UserId);

        modelBuilder.Entity<UserSession>()
            .HasOne(us => us.Session)
            .WithMany(s => s.UserSessions)
            .HasForeignKey(us => us.SessionId);
    }

}