using Classifieds.Auth;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DAL;

public class ClassifiedsDbContext: DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var a = new NpgsqlConnectionStringBuilder();
        // a.Host = Environment.GetEnvironmentVariable("POSTGRES_HOST");
        // a.Database = "classifieds";
        // a.Username = "postgres";
        // a.Password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
        a.Host = "localhost";
        a.Database = "classifieds";
        a.Username = "postgres";
        a.Password = "35135143s5fasfawf";
        optionsBuilder.UseNpgsql(a.ToString());
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>()
            .HasData(new List<Role>
            {
                new Role
                {
                    Id = (int) Roles.User,
                    Name = "Пользователь"
                },
                new Role
                {
                    Id = (int) Roles.Admin,
                    Name = "Админ"
                },
                new Role
                {
                    Id = (int) Roles.Moderator,
                    Name = "Модератор"
                }
            });
        modelBuilder.Entity<Category>()
            .HasData(new List<Category>
            {
                new Category
                {
                    Id = 1,
                    Name = "Транспорт",
                    SystemName = "Transport"
                },
                new Category
                {
                    Id = 2,
                    Name = "Электроника",
                    SystemName = "Electronic"
                },
                new Category
                {
                    Id = 3,
                    Name = "Смартфоны",
                    SystemName = "Smartphones",
                    ParentCategoryId = 2
                },
                new Category
                {
                    Id = 4,
                    Name = "Компьютеры",
                    SystemName = "Computers",
                    ParentCategoryId = 2
                },
                new Category
                {
                    Id = 5,
                    Name = "Автомобили",
                    SystemName = "Cars",
                    ParentCategoryId = 1
                },
                new Category
                {
                    Id = 6,
                    Name = "Мотоциклы",
                    SystemName = "Motorcycles",
                    ParentCategoryId = 1
                }
            });
        modelBuilder.Entity<UserProfile>()
            .HasData(new List<UserProfile>
            {
                new UserProfile
                {
                    Id = 1,
                    PhoneNumber = "admin",
                    PasswordHash =
                        "AQAAAAIAAYagAAAAEM7GBkpy8JY9wxcqEdXgHM7wYN1PsYrm5kbV7BmRUDkte+XTtNKyZP8tc67jpHMTug==",
                    Email = "admin@admin",
                    FirstName = "admin",
                    LastName = "admin"
                },
                new UserProfile
                {
                    Id = 2,
                    PhoneNumber = "moderator",
                    PasswordHash =
                        "AQAAAAIAAYagAAAAEEMFP0Xs+baC2Sd2P116RohHLSv10IRDtLIZUWCbNWGSSHCV6sFp8VQ3M5dZnZSAag==",
                    Email = "moderator@moderator",
                    FirstName = "moderator",
                    LastName = "moderator"
                }
            });
        modelBuilder.Entity<UserProfile>()
            .HasIndex(nameof(UserProfile.PhoneNumber)).IsUnique();
        
        modelBuilder.Entity<UserProfile>()
            .HasIndex(nameof(UserProfile.Email)).IsUnique();
        
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Account> Accounts { get; set; } = null!;
    public DbSet<AccountTransaction> AccountTransactions { get; set; } = null!;
    public DbSet<UserProfile> UserProfiles { get; set; } = null!;
    public DbSet<Listing> Listings { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;
}