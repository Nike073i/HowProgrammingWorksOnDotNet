using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HowProgrammingWorksOnDotNet.Database.Efc.ConfigAttr;

public abstract class BaseEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    // MSSQL
    [Timestamp]
    public byte[] TimeStamp { get; set; }

    /*
        Postgres:
        [Timestamp]
        public uint Version { get; set; }
    */
}

public class CarConfiguration : IEntityTypeConfiguration<Car>
{
    public void Configure(
        EntityTypeBuilder<Car> builder
    ) { /* .. */
    }
}

[Table("Inventory", Schema = "dbo")]
[Index(nameof(MakeId), Name = "IX_Inventory_MakeId")]
[EntityTypeConfiguration(typeof(CarConfiguration))]
public class Car : BaseEntity
{
    private string _color;

    [Required, StringLength(50)]
    public string Color
    {
        get => _color;
        set => _color = value;
    }

    private bool? _isDrivable;
    public bool IsDrivable
    {
        get => _isDrivable ?? true;
        set => _isDrivable = value;
    }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public string Display { get; set; }
    public DateTime? DateBuilt { get; set; }

    [Required, StringLength(50)]
    public string PetName { get; set; }
    public int MakeId { get; set; }

    [ForeignKey(nameof(MakeId))]
    public Make MakeNavigation { get; set; }
    public Radio RadioNavigation { get; set; }

    [InverseProperty(nameof(Driver.Cars))]
    public IEnumerable<Driver> Drivers { get; set; }

    [InverseProperty(nameof(CarDriver.CarNavigation))]
    public IEnumerable<CarDriver> CarDrivers { get; set; }
}

[Table("InventoryToDrivers", Schema = "dbo")]
public class CarDriver : BaseEntity
{
    public int DriverId { get; set; }

    [ForeignKey(nameof(DriverId))]
    public Driver DriverNavigation { get; set; }

    [Column("InventoryId")]
    public int CarId { get; set; }

    [ForeignKey(nameof(CarId))]
    public Car CarNavigation { get; set; }
}

[Table("Drivers", Schema = "dbo")]
public class Driver : BaseEntity
{
    public Person PersonInfo { get; set; }

    [InverseProperty(nameof(Car.Drivers))]
    public IEnumerable<Car> Cars { get; set; }

    [InverseProperty(nameof(CarDriver.DriverNavigation))]
    public IEnumerable<CarDriver> CarDrivers { get; set; }
}

[Table("Makes", Schema = "dbo")]
public class Make : BaseEntity
{
    [Required, StringLength(50)]
    public string Name { get; set; }

    [InverseProperty(nameof(Car.MakeNavigation))]
    public IEnumerable<Car> Cars { get; set; }
}

[Owned]
public class Person
{
    [Required, StringLength(50)]
    public string FirstName { get; set; }

    [Required, StringLength(50)]
    public string LastName { get; set; }
}

[Table("Radios", Schema = "dbo")]
public class Radio : BaseEntity
{
    public bool HasTweeters { get; set; }
    public bool HasSubWoofers { get; set; }

    [Required, StringLength(50)]
    public string RadioId { get; set; }

    [Column("InventoryId")]
    public int CarId { get; set; }

    [ForeignKey(nameof(CarId))]
    public Car CarNavigation { get; set; }
}
