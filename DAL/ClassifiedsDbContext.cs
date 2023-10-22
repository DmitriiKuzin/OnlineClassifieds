using Classifieds.Auth;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DAL;

public class ClassifiedsDbContext: DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var a = new NpgsqlConnectionStringBuilder();
        a.Host = Environment.GetEnvironmentVariable("POSTGRES_HOST");
        a.Database = "billing";
        a.Username = "postgres";
        a.Password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
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
                    Id = 0,
                    Name = "Транспорт",
                    SystemName = "Transport"
                },
                new Category
                {
                    Id = 1,
                    Name = "Электроника",
                    SystemName = "Electronic"
                },
                new Category
                {
                    Id = 2,
                    Name = "Смартфоны",
                    SystemName = "Smartphones",
                    ParentCategoryId = 1
                },
                new Category
                {
                    Id = 3,
                    Name = "Компьютеры",
                    SystemName = "Computers",
                    ParentCategoryId = 1
                },
                new Category
                {
                    Name = "Автомобили",
                    SystemName = "Cars",
                    ParentCategoryId = 0
                },
                new Category
                {
                    Name = "Мотоциклы",
                    SystemName = "Motorcycles",
                    ParentCategoryId = 0
                }
            });
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Account> Accounts { get; set; } = null!;
    public DbSet<AccountTransaction> AccountTransactions { get; set; } = null!;
    public DbSet<UserProfile> UserProfiles { get; set; } = null!;
    public DbSet<Listing> Listings { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
}