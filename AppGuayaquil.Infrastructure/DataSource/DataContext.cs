using AppGuayaquil.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AppGuayaquil.Infrastructure.DataSource;

public class DataContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<People> Peoples { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (modelBuilder == null)
        {
            throw new ArgumentNullException(nameof(modelBuilder));
        }

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.UserId);
            entity.Property(u => u.UserName).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Password).IsRequired();
            entity.Property(u => u.CreationDate).IsRequired();
        });

        
        modelBuilder.Entity<People>(entity =>
        {
            entity.HasKey(p => p.PeopleId);
            entity.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(p => p.LastName).IsRequired().HasMaxLength(100);
            entity.Property(p => p.IdentificationNumber).IsRequired().HasMaxLength(50);
            entity.Property(p => p.IdentificationType).IsRequired().HasMaxLength(20);
            entity.Property(p => p.Email).IsRequired().HasMaxLength(150);
            entity.Property(p => p.CreationDate).IsRequired();
            
            entity.Ignore(p => p.FullIdentification);
            entity.Ignore(p => p.FullName);
        });
        
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(DomainEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property<DateTime>("CreatedOn")
                    .IsRequired();

                modelBuilder.Entity(entityType.ClrType)
                    .Property<DateTime>("LastModifiedOn")
                    .IsRequired();
            }
        }

        base.OnModelCreating(modelBuilder);
    }
}
