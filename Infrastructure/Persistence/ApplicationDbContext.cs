using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet for TaskItem entity
        public DbSet<TaskItem> TaskItems { get; set; } = default!;
        // DbSet for TaskStatusUpdate entity
        public DbSet<TaskStatusUpdate> TaskStatusUpdates { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // Important: Call base method first

            // TaskItem Entity Configuration
            builder.Entity<TaskItem>(entity =>
            {
                entity.HasKey(ti => ti.Id); // Primary Key

                entity.Property(ti => ti.Title)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(ti => ti.Description)
                      .HasMaxLength(1000);

                entity.Property(ti => ti.RewardPrice)
                      .HasColumnType("decimal(18,2)"); // Specify SQL column type for precision

                // Relationship: TaskItem created by AppUser (Many-to-One)
                entity.HasOne(ti => ti.CreatedBy) // Navigation property in TaskItem
                      .WithMany(u => u.CreatedTasks) // Collection navigation property in AppUser
                      .HasForeignKey(ti => ti.CreatedById) // Foreign key property in TaskItem
                      .OnDelete(DeleteBehavior.Restrict); // Prevent deleting AppUser if they have created tasks

                // Relationship: TaskItem assigned to AppUser (Many-to-One, optional)
                entity.HasOne(ti => ti.Assignee) // Navigation property in TaskItem
                      .WithMany(u => u.AssignedTasks) // Collection navigation property in AppUser
                      .HasForeignKey(ti => ti.AssigneeId) // Foreign key property in TaskItem
                      .IsRequired(false) // AssigneeId can be null
                      .OnDelete(DeleteBehavior.SetNull); // If assigned AppUser is deleted, set AssigneeId to null

                // Relationship: TaskItem has many TaskStatusUpdates (One-to-Many)
                entity.HasMany(ti => ti.StatusUpdates) // Collection navigation property in TaskItem
                      .WithOne(tsu => tsu.TaskItem) // Navigation property in TaskStatusUpdate
                      .HasForeignKey(tsu => tsu.TaskItemId) // Foreign key property in TaskStatusUpdate
                      .OnDelete(DeleteBehavior.Cascade); // If TaskItem is deleted, cascade delete its StatusUpdates

                // Enum to string conversion for TaskItemStatus
                entity.Property(ti => ti.Status)
                      .HasConversion(
                          status => status.ToString(), // Convert enum to string for DB storage
                          value => (TaskItemStatus)Enum.Parse(typeof(TaskItemStatus), value) // Convert string from DB to enum
                      );

                entity.Property(ti => ti.CreatedAt)
                      .ValueGeneratedOnAdd() // Ensures DateTime is generated on add
                      .HasDefaultValueSql("GETUTCDATE()"); // SQL Server specific, use appropriate function for other DBs

                entity.Property(ti => ti.UpdatedAt)
                      .ValueGeneratedOnAddOrUpdate() // Ensures DateTime is generated on add or update
                      .IsRequired(false); // Can be null initially
            });

            // TaskStatusUpdate Entity Configuration
            builder.Entity<TaskStatusUpdate>(entity =>
            {
                entity.HasKey(tsu => tsu.Id); // Primary Key

                entity.Property(tsu => tsu.Comment)
                      .IsRequired()
                      .HasMaxLength(500);

                // Relationship: TaskStatusUpdate updated by AppUser (Many-to-One, optional)
                entity.HasOne(tsu => tsu.UpdatedBy) // Navigation property in TaskStatusUpdate
                      .WithMany() // AppUser might not have a direct ICollection of TaskStatusUpdates they've made
                      .HasForeignKey(tsu => tsu.UpdatedById) // Foreign key property in TaskStatusUpdate
                      .IsRequired(false) // UpdatedById can be null
                      .OnDelete(DeleteBehavior.SetNull); // If updating AppUser is deleted, set UpdatedById to null

                // Enum to string conversion for TaskItemStatus in TaskStatusUpdate
                entity.Property(tsu => tsu.Status)
                      .HasConversion(
                          status => status.ToString(),
                          value => (TaskItemStatus)Enum.Parse(typeof(TaskItemStatus), value)
                      );

                entity.Property(tsu => tsu.UpdatedAt)
                      .ValueGeneratedOnAdd()
                      .HasDefaultValueSql("GETUTCDATE()");
            });

            // You can customize ASP.NET Identity table names and schema here if needed
            // For example:
            // builder.Entity<AppUser>(entity => { entity.ToTable(name: "Users", schema: "identity"); });
            // builder.Entity<IdentityRole>(entity => { entity.ToTable(name: "Roles", schema: "identity"); });
            // builder.Entity<IdentityUserRole<string>>(entity => { entity.ToTable("UserRoles", schema: "identity"); });
            // builder.Entity<IdentityUserClaim<string>>(entity => { entity.ToTable("UserClaims", schema: "identity"); });
            // builder.Entity<IdentityUserLogin<string>>(entity => { entity.ToTable("UserLogins", schema: "identity"); });
            // builder.Entity<IdentityRoleClaim<string>>(entity => { entity.ToTable("RoleClaims", schema: "identity"); });
            // builder.Entity<IdentityUserToken<string>>(entity => { entity.ToTable("UserTokens", schema: "identity"); });
        }
    }
}
