using Microsoft.EntityFrameworkCore;

namespace HowProgrammingWorksOnDotNet.Database.Efc;

/*
    Relationships:
        Car 0..M -> 1 Make
        Car 1 -> 0..1 Radio
        Car 1 -> 0..M CarDriver
*/
public class Car
{
    public int Id { get; set; }
    public string Color { get; set; }
    public bool? IsDrivable { get; set; }

    public string Display { get; set; }
    public DateTime? DateBuilt { get; set; }

    public string PetName { get; set; }

    public int MakeId { get; set; }
    public Make Make { get; set; }

    public int? RadioId { get; set; }
    public Radio? Radio { get; set; }

    public IEnumerable<CarDriver> CarDrivers { get; set; }
}

/*
    Relationships:
    CarDriver 0..M -> 1 Driver
    CarDriver 0..M -> 1 Car
*/
public class CarDriver
{
    public int Id { get; set; }

    public int DriverId { get; set; }
    public Driver Driver { get; set; }

    public int CarId { get; set; }
    public Car Car { get; set; }
}

/*
    Relationships:
    Driver 1 -> 0..M CarDriver
    Person part of Driver (Logical separation, one table)
*/
public class Driver
{
    public int Id { get; set; }
    public Person PersonInfo { get; set; }
    public IEnumerable<CarDriver> CarDrivers { get; set; }
}

/*
    Relationships:
    Make 1 -> 0..M  Car
*/
public class Make
{
    public int Id { get; set; }
    public string Name { get; set; }

    public IEnumerable<Car> Cars { get; set; }
}

[Owned]
public class Person
{
    public string FirstName { get; set; }

    public string LastName { get; set; }
}

/*
    Radio 0..1 -> 1 Car
*/
public class Radio
{
    // Prinary key, Foreign key
    public int CarId { get; set; }
    public bool HasTweeters { get; set; }
    public bool HasSubWoofers { get; set; }
    public Car Car { get; set; }
}
