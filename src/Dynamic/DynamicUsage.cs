using System.Diagnostics;
using System.Security;
using Microsoft.CSharp.RuntimeBinder;

namespace HowProgrammingWorksOnDotNet.Dynamic;

public class User(string name, int age)
{
    public string Name { get; } = name;
    public int Age { get; } = age;

    public string Report(char separator, int separatorRepeat, string prefix) =>
        string.Concat(prefix, Name, new string(separator, separatorRepeat), Age);

    public int AgeDifference(Vasya vasya) => Age - vasya.Age;

    public string Method1(Vasya vasya, int number) => "M1 with Vasya";
}

public class Vasya : User
{
    public Vasya()
        : base("Vasya", 40) { }

    public void SpecialMethod(int x) { }
}

public class DynamicUsage
{
    [Fact]
    public void NoMemberValidation()
    {
        var nikita = new User("Nikita", 15);
        string expectedReport = "pre: Nikita---15";

        string report = nikita.Report('-', 3, "pre: ");
        Assert.Equal(expectedReport, report);

        dynamic dynNik = nikita;
        Assert.Throws<RuntimeBinderException>(() => dynNik.X());
        Assert.Throws<RuntimeBinderException>(() => dynNik.Report());
        Assert.Throws<RuntimeBinderException>(() => dynNik.Report("s", 3, "pre: ")); // cz its (char, int, string), but in invoke - (str, int, string)

        string dynReport = dynNik.Report('-', 3, "pre: ");
        Assert.Equal(expectedReport, dynReport);

        User vasya = new Vasya();
        // vasya.SpecialMethod(5); // Compiler Error
        dynamic dVasya = vasya;
        dVasya.SpecialMethod(5);
    }

    [Fact]
    public void NoDynamicArgsValidation()
    {
        User nikita = new User("Nikita", 15);
        dynamic dynamicNikita = nikita;
        Vasya vasya = new();
        User userVasya = vasya;
        dynamic dynamicVasya = vasya;
        dynamic dynamicUserVasya = userVasya;

        int dif = nikita.Age - vasya.Age;

        Assert.Equal(nikita.AgeDifference(vasya), dif);

        // nikita.AgeDifference(userVasya); - Compile Error, cz Method expects Vasya, but gets User
        Assert.Throws<RuntimeBinderException>(() => dynamicNikita.AgeDifference(userVasya)); // No Compile Error, but Runtime error, cz Method expects Vasya, but gets User
        Assert.Equal(nikita.AgeDifference(dynamicVasya), dif); // No error, but method expects Vasya and gets Dynamic-Wrapper
        Assert.Equal(nikita.AgeDifference(dynamicUserVasya), dif); // NO ERROR! Метод ожидает Vasya, но в него приходит dynamic-обертка типа User. Однако User это ссылка на объект, **реальный тип** которого на самом деле - Vasya
        Assert.Equal(dynamicNikita.AgeDifference(dynamicUserVasya), dif); // No Error

        // nikita.Method1(dynamicNikita, "") - CompileError. Method1::(Vasya, int), но вместо int приходит str.  Vasya не проверяется
    }
}
