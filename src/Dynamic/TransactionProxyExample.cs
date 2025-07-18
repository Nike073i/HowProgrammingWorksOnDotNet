using System.Dynamic;

namespace HowProgrammingWorksOnDotNet.Dynamic;

public class TransactionProxy(object target) : DynamicObject
{
    private readonly Dictionary<string, object?> _modified = [];

    public override bool TryGetMember(GetMemberBinder binder, out object? result)
    {
        string memberName = binder.Name;

        if (_modified.TryGetValue(memberName, out result))
            return true;

        var property = target.GetType().GetProperty(memberName);
        if (property != null)
        {
            result = property.GetValue(target);
            return true;
        }

        result = null;
        return true;
    }

    public override bool TrySetMember(SetMemberBinder binder, object? value)
    {
        _modified[binder.Name] = value;
        return true;
    }

    public void Rollback() => _modified.Clear();

    public void Commit()
    {
        foreach (var pair in _modified)
        {
            var property = target.GetType().GetProperty(pair.Key);
            property?.SetValue(target, pair.Value);
        }
        _modified.Clear();
    }
}

public class TransactionProxyTests
{
    private record User(string Name, int Age);

    [Fact]
    public void AccessWithoutModifications()
    {
        var target = new User("Nikita", 15);
        dynamic proxy = new TransactionProxy(target);

        Assert.Equal(target.Name, proxy.Name);
        Assert.Equal(target.Age, proxy.Age);
    }

    [Fact]
    public void AccessWithModifications()
    {
        var target = new User("Nikita", 15);
        dynamic proxy = new TransactionProxy(target);

        proxy.Name = "Nekita";
        proxy.Age = 100;

        Assert.Equal("Nekita", proxy.Name);
        Assert.Equal(100, proxy.Age);
    }

    [Fact]
    public void AccessWithCommitedChanges()
    {
        var target = new User("Nikita", 15);
        dynamic proxy = new TransactionProxy(target);
        proxy.Name = "NewName";
        proxy.Age = 100;

        proxy.Commit();

        Assert.Equal("NewName", target.Name);
        Assert.Equal(100, target.Age);
        Assert.Equal("NewName", proxy.Name);
    }

    [Fact]
    public void AccessWithDiscardedChanges()
    {
        var target = new User("Nikita", 15);
        dynamic proxy = new TransactionProxy(target);
        proxy.Name = "NewName";
        proxy.Age = 100;

        proxy.Rollback();

        Assert.Equal("Nikita", proxy.Name);
        Assert.Equal(15, proxy.Age);
        Assert.Equal("Nikita", target.Name);
        Assert.Equal(15, target.Age);
    }

    [Fact]
    public void CommitAndRollback()
    {
        var target = new User("Nikita", 15);
        dynamic proxy = new TransactionProxy(target);
        proxy.Name = "NewName";

        proxy.Commit();
        proxy.Name = "AnotherName";
        proxy.Rollback();

        Assert.Equal("NewName", proxy.Name);
    }
}
