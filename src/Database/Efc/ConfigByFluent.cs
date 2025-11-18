using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HowProgrammingWorksOnDotNet.Database.Efc.ConfigFluent;

public class DomainDbContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Кастомное ограничение, что производитель не равен Lemon...
        modelBuilder
            .Entity<Make>()
            .ToTable("Makes", "dbo", t => t.HasCheckConstraint("CK_Check_Name", "[Name]<>'Lemon'"));

        // Не участвует в миграции
        modelBuilder.Entity<LogEntry>().ToTable(action => action.ExcludeFromMigrations());
    }
}

public record LogEntry(int Id, string Text);

public class CarConfiguration : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.ToTable("Inventory", "dbo");

        builder.HasKey(e => e.Id);
        /*
            CompositeKey: builder.HasKey(e => new { e.Id, e.AnotherField });
        */
        builder.HasIndex(e => e.MakeId, "IX_Inventory_MakeId");
        builder.Property(e => e.Color).IsRequired().HasMaxLength(50).HasDefaultValue("Black");
        builder.Property(e => e.PetName).IsRequired().HasMaxLength(50);

        // MSSQL. Postgres: now()
        builder.Property(e => e.DateBuilt).HasDefaultValueSql("getdate()");

        builder.Property(e => e.IsDrivable).HasField("_isDrivable").HasDefaultValue(true); // Использование nullable-резервного поля во избежание дефолтного false от C#

        builder.Property(e => e.TimeStamp).IsRowVersion().IsConcurrencyToken();

        builder
            .Property(e => e.Display)
            .HasComputedColumnSql("[PetName] + ' (' + [Color] + ')'", stored: true);
        builder
            .HasOne(d => d.MakeNavigation)
            .WithMany(p => p.Cars)
            .HasForeignKey(d => d.MakeId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Inventory_Makes_MakeId");

        builder
            .HasMany(p => p.Drivers)
            .WithMany(p => p.Cars)
            .UsingEntity<CarDriver>(
                j =>
                    j.HasOne(cd => cd.DriverNavigation)
                        .WithMany(d => d.CarDrivers)
                        .HasForeignKey(nameof(CarDriver.DriverId))
                        .HasConstraintName("FK_InventoryDriver_Drivers_DriverId")
                        .OnDelete(DeleteBehavior.Cascade),
                j =>
                    j.HasOne(cd => cd.CarNavigation)
                        .WithMany(c => c.CarDrivers)
                        .HasForeignKey(nameof(CarDriver.CarId))
                        .HasConstraintName("FK_InventoryDriver_Inventory_InventoryId")
                        .OnDelete(DeleteBehavior.ClientCascade),
                j =>
                {
                    j.HasKey(cd => new { cd.CarId, cd.DriverId });
                }
            );
    }
}

public class DriverConfiguration : IEntityTypeConfiguration<Driver>
{
    public void Configure(EntityTypeBuilder<Driver> builder)
    {
        builder.OwnsOne(
            o => o.PersonInfo,
            pd =>
            {
                pd.Property<string>(nameof(Person.FirstName))
                    .HasColumnName(nameof(Person.FirstName))
                    .HasColumnType("nvarchar(50)");
                pd.Property<string>(nameof(Person.LastName))
                    .HasColumnName(nameof(Person.LastName))
                    .HasColumnType("nvarchar(50)");
            }
        );
        builder.Navigation(d => d.PersonInfo).IsRequired(true);
    }
}

public class RadioConfiguration : IEntityTypeConfiguration<Radio>
{
    public void Configure(EntityTypeBuilder<Radio> builder)
    {
        builder.Property(e => e.CarId).HasColumnName("InventoryId");
        builder.HasIndex(e => e.CarId, "IX_Radios_CarId").IsUnique();
        builder
            .HasOne(d => d.CarNavigation)
            .WithOne(p => p.RadioNavigation)
            .HasForeignKey<Radio>(d => d.CarId);
    }
}

public class AppDbContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
}

public abstract class BaseEntity
{
    public int Id { get; set; }
    public byte[] TimeStamp { get; set; }
}

public class Car : BaseEntity
{
    public string Color { get; set; }

    private bool? isDrivable;
    public bool IsDrivable
    {
        get => isDrivable ?? true; // default value in C#
        set => isDrivable = value;
    }

    public string Display { get; set; }
    public DateTime? DateBuilt { get; set; }

    public string PetName { get; set; }
    public int MakeId { get; set; }

    public Make MakeNavigation { get; set; }
    public Radio RadioNavigation { get; set; }

    public IEnumerable<Driver> Drivers { get; set; }

    public IEnumerable<CarDriver> CarDrivers { get; set; }
}

public class CarDriver : BaseEntity
{
    public int DriverId { get; set; }

    public Driver DriverNavigation { get; set; }

    public int CarId { get; set; }

    public Car CarNavigation { get; set; }
}

public class Driver : BaseEntity
{
    public Person PersonInfo { get; set; }
    public IEnumerable<Car> Cars { get; set; }
    public IEnumerable<CarDriver> CarDrivers { get; set; }
}

public class Make : BaseEntity
{
    public string Name { get; set; }
    public IEnumerable<Car> Cars { get; set; }
}

public class Person
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class Radio : BaseEntity
{
    public bool HasTweeters { get; set; }
    public bool HasSubWoofers { get; set; }
    public string RadioId { get; set; }
    public int CarId { get; set; }
    public Car CarNavigation { get; set; }
}
