using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Loader;

namespace HowProgrammingWorksOnDotNet.Dlr;

/*
    REQ:
    - System.Reflection.Emit
    - .net 9+

*/

/*
    public class HelloWorld
    {
        private string theMessage;
        public HelloWorld() { }
        public HelloWorld(string s) { theMessage = s; }
        public string GetMsg() => theMessage;
        public void SayHello() => Console.WriteLine("Hello !");
    }
*/

public class DynamicAssembly
{
    [Fact]
    public void Persisted()
    {
        var assemblyBuilder = new PersistedAssemblyBuilder(
            new AssemblyName { Name = "MyTestAss", Version = new("1.2.3.4") },
            typeof(object).Assembly
        );

        var moduleBuilder = assemblyBuilder.DefineDynamicModule("MyTestAss");
        CreateHelloCls(moduleBuilder);
        using (var fileStream = new FileStream("persisted.dll", FileMode.OpenOrCreate))
        {
            assemblyBuilder.Save(fileStream);
        }

        // ...
        using (var fileStream = new FileStream("persisted.dll", FileMode.Open))
        {
            var assembly = AssemblyLoadContext.Default.LoadFromStream(fileStream);
            var helloCls = assembly.GetExportedTypes().First();
            TestHelloCls(helloCls);
        }
    }

    [Fact]
    public void InMemory()
    {
        var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
            new AssemblyName { Name = "MyTestAss", Version = new("1.2.3.4") },
            AssemblyBuilderAccess.Run
        );

        var moduleBuilder = assemblyBuilder.DefineDynamicModule("MyTestAss");
        var helloCls = CreateHelloCls(moduleBuilder);
        TestHelloCls(helloCls);
    }

    private void TestHelloCls(Type helloCls)
    {
        dynamic helloObj = Activator.CreateInstance(helloCls, "строка")!;
        Console.WriteLine($"результат GetMsg - {helloObj.GetMsg()} ");
        helloObj.SayHello();
    }

    private Type CreateHelloCls(ModuleBuilder moduleBuilder)
    {
        var helloWorldClsBuilder = moduleBuilder.DefineType(
            $"{moduleBuilder.Name}.HelloWorld",
            TypeAttributes.Public
        );
        var baseCls = typeof(object);

        var fieldBuilder = helloWorldClsBuilder.DefineField(
            "theMessage",
            typeof(string),
            FieldAttributes.Private
        );

        Type[] ctorArgs = [typeof(string)];
        var ctorBuilder = helloWorldClsBuilder.DefineConstructor(
            MethodAttributes.Public,
            CallingConventions.Standard,
            ctorArgs
        );
        var baseCtorInfo = baseCls.GetConstructor([])!;
        var ctorIlGen = ctorBuilder.GetILGenerator();
        ctorIlGen.Emit(OpCodes.Ldarg_0); // this
        ctorIlGen.Emit(OpCodes.Call, baseCtorInfo); // call base ctor: this.base();
        ctorIlGen.Emit(OpCodes.Ldarg_0); // this
        ctorIlGen.Emit(OpCodes.Ldarg_1); // string_arg
        ctorIlGen.Emit(OpCodes.Stfld, fieldBuilder); // store field: this.theMessage = string_arg
        ctorIlGen.Emit(OpCodes.Ret);

        helloWorldClsBuilder.DefineDefaultConstructor(MethodAttributes.Public);

        var getMsgBuilder = helloWorldClsBuilder.DefineMethod(
            "GetMsg",
            MethodAttributes.Public,
            typeof(string),
            null // without args
        );

        var getMsgIlGen = getMsgBuilder.GetILGenerator();
        getMsgIlGen.Emit(OpCodes.Ldarg_0); // this
        getMsgIlGen.Emit(OpCodes.Ldfld, fieldBuilder); // stack = this.theMessage
        getMsgIlGen.Emit(OpCodes.Ret);

        var sayHelloBuilder = helloWorldClsBuilder.DefineMethod(
            "SayHello",
            MethodAttributes.Public,
            null, // void
            null // without args
        );

        var sayHelloIlGen = sayHelloBuilder.GetILGenerator();
        sayHelloIlGen.EmitWriteLine("Hello!");
        sayHelloIlGen.Emit(OpCodes.Ret);

        return helloWorldClsBuilder.CreateType();
    }
}
