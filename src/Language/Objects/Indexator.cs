namespace HowProgrammingWorksOnDotNet.Language.Objects;

// Бессмысленный пример определенияя индексатора в классе

public record Person(int Id, string Name);

public class PeopleCollection(int capacity)
{
    private readonly Person[] _people = new Person[capacity];
    public Person this[int index]
    {
        get => _people[index];
        set => _people[index] = value;
    }
}
