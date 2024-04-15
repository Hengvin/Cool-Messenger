using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SPG.Messenger.Domain.MediaDomain;
using SPG.Messenger.Domain.Model.Messenger;
using SPG.Messenger.Domain.Model.MessengerDomain;
using SPG.Messenger.Domain.Model.UserDomain;
namespace SPG.Messenger.Infrastructure;

public class MessengerContext : DbContext
{
    // MessengerEntry ----------------------------------------------------------
    public DbSet<MessengerEntry> MessengerEntries => Set<MessengerEntry>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<Media> Media => Set<Media>();


    // User --------------------------------------------------------------------
    public DbSet<User> Users => Set<User>();
    public DbSet<Relation> Relations => Set<Relation>();



    public MessengerContext() { }

    public MessengerContext(DbContextOptions options)
        : base(options)
    {
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // TODO in memory for testing only
            optionsBuilder.UseSqlite("DataSource=:memory:");
        }
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // MessengerEntry ------------------------------------------------------

        modelBuilder.Entity<MessengerEntry>()
            .HasMany(m => m.Messages)
            .WithOne(m => m.MessengerNavigation)
            .HasForeignKey("MessengerEntryId");

        modelBuilder.Entity<Message>()
            .HasOne(m => m.Media)
            .WithOne()
            .HasForeignKey<Message>("MediaId");


        // User ----------------------------------------------------------------

        modelBuilder.Entity<User>()
            .HasMany(m => m.Messengers)
            .WithOne(u => u.UserNavigation)
            .HasForeignKey("UserId");

        modelBuilder.Entity<User>()
            .HasMany<Message>()
            .WithOne(m => m.SenderNavigation)
            .HasForeignKey("UserId");


        // Relation represents a many-to-many relationship between Users.
        // Each Relation links a Self User to a Friend User (and vice versa).
        // This is modeled as two one-to-many relationships from Relation to User.

        // One-to-many relationship from Relation (Self side) to User.
        modelBuilder.Entity<Relation>()
            .HasOne(r => r.Self)
            .WithMany()
            .HasForeignKey("SelfId")
            .OnDelete(DeleteBehavior.Restrict);

        // One-to-many relationship from Relation (Friend side) to User.
        modelBuilder.Entity<Relation>()
            .HasOne(r => r.Friend)
            .WithMany()
            .HasForeignKey("FriendId")
            .OnDelete(DeleteBehavior.Restrict);


        // Value Objects
        modelBuilder.Entity<User>().OwnsOne(u => u.Profile);
        modelBuilder.Entity<User>().OwnsOne(u => u.Account);
    }

}
