using Microsoft.EntityFrameworkCore;
using TicketManagement.API.Models.Entities;

namespace TicketManagement.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Ticket> Tickets { get; set; }

    public DbSet<TicketComment> TicketComments { get; set; }

    public DbSet<TicketStatusHistory> TicketStatusHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Enum -> string
        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasConversion<string>();

        modelBuilder.Entity<Ticket>()
            .Property(t => t.Status)
            .HasConversion<string>();

        modelBuilder.Entity<Ticket>()
            .Property(t => t.Priority)
            .HasConversion<string>();

        // User configuration
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .Property(u => u.FullName)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .HasMaxLength(150)
            .IsRequired();

        // Ticket configuration
        modelBuilder.Entity<Ticket>()
            .HasIndex(t => t.TicketNumber)
            .IsUnique();

        modelBuilder.Entity<Ticket>()
            .Property(t => t.TicketNumber)
            .HasMaxLength(30)
            .IsRequired();

        modelBuilder.Entity<Ticket>()
            .Property(t => t.Title)
            .HasMaxLength(200)
            .IsRequired();

        // User -> Tickets : CreatedBy
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.CreatedByUser)
            .WithMany(u => u.CreatedTickets)
            .HasForeignKey(t => t.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // User -> Tickets : AssignedTo
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.AssignedToUser)
            .WithMany(u => u.AssignedTickets)
            .HasForeignKey(t => t.AssignedToUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Ticket -> Comments
        modelBuilder.Entity<TicketComment>()
            .HasOne(c => c.Ticket)
            .WithMany()
            .HasForeignKey(c => c.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TicketComment>()
            .HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Ticket -> Status History
        modelBuilder.Entity<TicketStatusHistory>()
            .HasOne(h => h.Ticket)
            .WithMany()
            .HasForeignKey(h => h.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TicketStatusHistory>()
            .HasOne(h => h.ChangedByUser)
            .WithMany()
            .HasForeignKey(h => h.ChangedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}