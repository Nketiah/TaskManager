using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Identity;



namespace TaskManager.Infrastructure.Persistence
{
    public class TaskManagerDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options) : base(options)
        {
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<TeamInvitation> TeamInvitations { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Team>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Name).IsRequired().HasMaxLength(100);
            });

            builder.Entity<Member>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.HasOne(m => m.Team)
                      .WithMany(t => t.Members)
                      .HasForeignKey(m => m.TeamId)
                      .OnDelete(DeleteBehavior.Cascade); 
            });

            builder.Entity<TaskItem>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Title)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(t => t.Status)
                      .HasConversion<string>();

                entity.HasOne(t => t.AssignedTo)
                      .WithMany()
                      .HasForeignKey(t => t.AssignedToMemberId)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(t => t.Team)
                      .WithMany(t => t.Tasks) 
                      .HasForeignKey(t => t.TeamId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<TeamInvitation>(entity =>
            {
                entity.HasKey(i => i.Id);
                entity.HasIndex(i => i.Email);
                entity.HasOne(i => i.Team)
                      .WithMany()
                      .HasForeignKey(i => i.TeamId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<TaskItem>()
                   .HasOne(t => t.AssignedTo)
                   .WithMany(m => m.AssignedTasks)
                   .HasForeignKey(t => t.AssignedToMemberId)
                   .OnDelete(DeleteBehavior.Restrict);
            }

    }
}
