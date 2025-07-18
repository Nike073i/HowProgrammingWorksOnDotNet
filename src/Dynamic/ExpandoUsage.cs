using System.ComponentModel;
using System.Dynamic;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.CSharp.RuntimeBinder;

namespace HowProgrammingWorksOnDotNet.Dynamic;

public class ExpandoUsage
{
    [Fact]
    public void AdaptiveObject()
    {
        dynamic user = new ExpandoObject();
        user.Age = 15;
        user.Name = "Nikita";
        user.Fn = new Func<int, int>(x => user.Age * x);

        Assert.Equal(75, user.Fn(5));
        user.Age++;
        Assert.Equal(80, user.Fn(5));

        bool agePropChanged = false;
        ((INotifyPropertyChanged)user).PropertyChanged += (sender, args) =>
            agePropChanged = args.PropertyName == "Age";
        ((IDictionary<string, object>)user).Remove("Age");
        Assert.True(agePropChanged);
        Assert.Throws<RuntimeBinderException>(() => user.Age);
        Assert.False(((IDictionary<string, object>)user).ContainsKey("Age"));

        Assert.Throws<RuntimeBinderException>(() => user.Address.City = "Ulyanovsk");
        user.Address = new ExpandoObject();
        user.Address.City = "Ulyanovsk";
    }

    [Fact]
    public void Alternative()
    {
        var dict = new Dictionary<string, dynamic> { ["Age"] = 15 };
        dict["Fn"] = new Func<int, int>(x => dict["Age"] * x);
        Assert.Equal(75, dict["Fn"](5));
        // Если нужен INotifyPropertyChanged, то можно создать свою обертку или использовать сторонние решения
    }

    [Fact]
    public void LoadFromStorageExample()
    {
        Func<string> EntryToString(ExpandoObject entry) =>
            () => string.Join(", ", entry.Select(kvp => $"{kvp.Key} = {kvp.Value}"));

        ExpandoObject[] LoadData()
        {
            string responseFromExternalService = """
                [
                    {
                        "age": 15,
                        "name": "Nikita"
                    },
                    {
                        "age": 16,
                        "name": "Nekita"
                    }
                ]
                """;
            ExpandoObject[] data = JsonSerializer.Deserialize<ExpandoObject[]>(
                responseFromExternalService
            )!;
            foreach (dynamic entry in data)
                entry.ToString = new Func<string>(EntryToString(entry));
            return data;
        }

        var data = LoadData();
        foreach (dynamic entry in data)
            Console.WriteLine(entry.ToString());

        foreach (dynamic entry in data)
        {
            entry.Id = Guid.NewGuid();
            Console.WriteLine(entry.ToString());
        }
    }
}
