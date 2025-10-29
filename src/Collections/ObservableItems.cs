namespace HowProgrammingWorksOnDotNet.Collections;

using System.Collections.ObjectModel;

public record User(int Id, string Code);

public class ObservableItems
{
    [Fact]
    public void Usage()
    {
        var users = Enumerable
            .Range(1, 25)
            .Select(i => new User(i, Guid.NewGuid().ToString()))
            .ToList();

        var obs = new ObservableCollection<User>(users.Take(5));

        obs.CollectionChanged += (sender, args) =>
        {
            Console.WriteLine(new string('=', 80));
            Console.WriteLine(args.Action);
            if (args.NewItems != null)
                Console.WriteLine(
                    $"New item[{args.NewStartingIndex}]: {args.NewItems.Cast<User>().First()}"
                );
            if (args.OldItems != null)
                Console.WriteLine(
                    $"Old item[{args.OldStartingIndex}]: {args.OldItems.Cast<User>().First()}"
                );
        };

        users.Skip(5).Take(5).ToList().ForEach(obs.Add);

        var firstItem = obs[0];
        obs.Remove(firstItem);
        obs.RemoveAt(2);
        obs.RemoveAt(6);
        obs[4] = users.Last();
        obs.Move(0, 2);
        obs.Clear();
    }
}
