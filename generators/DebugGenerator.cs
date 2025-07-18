using Microsoft.CodeAnalysis;

namespace generators;

[Generator]
public class DebugGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(context =>
        {
            context.AddSource("debug.g.cs", "// debug-file");
        });
    }
}
