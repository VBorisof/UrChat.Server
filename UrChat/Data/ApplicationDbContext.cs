using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UrChat.Data.Models;

namespace UrChat.Data
{
    public sealed class ApplicationDbContext : DbContext
    {
        // Make sure to provide DbSets even for static fields that we won't 
        // query - Entity Framework seems to resort to the name of the entity
        // for a table unless it finds a DbSet of that entity. Then it uses 
        // that DbSets' name, which is probably how we want it.

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        
        public ApplicationDbContext(DbContextOptions options) : base(options)
        { }
        
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<User>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                e.Property(x => x.CreatedAt)
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("now() at time zone 'utc'");
                e.Property(x => x.ModifiedAt)
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("now() at time zone 'utc'");
            });
            
            builder.Entity<Message>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                e.Property(x => x.CreatedAt)
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("now() at time zone 'utc'");
                e.Property(x => x.ModifiedAt)
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("now() at time zone 'utc'");

                e.HasOne(u => u.Sender)
                    .WithMany();
            });
        }

        public override int SaveChanges()
        {
            var currentDateTime = DateTime.UtcNow;
            var entries =
                ChangeTracker.Entries()
                    .Where(e => e.Entity is IModelBase &&
                                (e.State == EntityState.Added || e.State == EntityState.Modified)).ToList();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    ((IModelBase) entry.Entity).CreatedAt = currentDateTime;
                }
                ((IModelBase) entry.Entity).ModifiedAt = currentDateTime;
            }

            var modifiedCount = 0;
            try
            {
                modifiedCount = base.SaveChanges();
            }
            catch
            {
                // TODO: Log error
            }

            return modifiedCount;
        }
    }
}
