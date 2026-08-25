using AsharibKamal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AsharibKamal.Infrastructure.Persistence;

public sealed class PortfolioDbContext(DbContextOptions<PortfolioDbContext> options) : DbContext(options)
{
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<BlogPost> BlogPosts => Set<BlogPost>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();
    public DbSet<Subscriber> Subscribers => Set<Subscriber>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasIndex(x => x.Slug).IsUnique();
            entity.Property(x => x.Title).HasMaxLength(180);
            entity.Property(x => x.Slug).HasMaxLength(180);
        });

        modelBuilder.Entity<BlogPost>(entity =>
        {
            entity.HasIndex(x => x.Slug).IsUnique();
            entity.Property(x => x.Title).HasMaxLength(220);
            entity.Property(x => x.Slug).HasMaxLength(220);
            entity.Property(x => x.CoverImageUrl).HasMaxLength(1000);
            entity.Property(x => x.SeoTitle).HasMaxLength(70);
            entity.Property(x => x.SeoDescription).HasMaxLength(170);
        });

        modelBuilder.Entity<Subscriber>(entity =>
        {
            entity.HasIndex(x => x.Email).IsUnique();
            entity.Property(x => x.Email).HasMaxLength(320);
        });
    }
}
