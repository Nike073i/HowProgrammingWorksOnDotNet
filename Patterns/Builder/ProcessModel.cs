using System.Diagnostics;

namespace HowProgrammingWorksOnDotNet.Patterns.Builder;

public class ProcessModel
{
    private readonly Process _process;

    private ProcessModel(Process process) => _process = process;

    // Another methods, properties...

    public class Builder(string utilName)
    {
        private readonly Dictionary<string, object?> _args = [];
        private readonly List<string> _options = [];

        public Builder AddArg(string name, object value)
        {
            _args[name] = value;
            return this;
        }

        public Builder AddFlag(string name)
        {
            _args[name] = null;
            return this;
        }

        public Builder AddOption(string option)
        {
            _options.Add(option);
            return this;
        }

        public ProcessModel Build()
        {
            var parameters = _args
                .Select(kv =>
                {
                    var prefix = kv.Key.Length > 1 ? "--" : "-";
                    return $"{prefix}{kv.Key} {kv.Value}";
                })
                .ToList();
            parameters.AddRange(_options);

            var args = string.Join(" ", parameters);

            var process = Process.Start(utilName, args);

            return new(process);
        }
    }
}

public class BuilderPatternExample
{
    [Fact]
    public void Usage()
    {
        var processBuilder = new ProcessModel.Builder("curl");
        processBuilder
            .AddFlag("v")
            .AddFlag("s")
            .AddFlag("include")
            .AddArg("output", "/home/skuld/tmp/curl-request.txt")
            .AddOption("https://google.com");
        var _ = processBuilder.Build();
    }
}
