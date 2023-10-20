using CommandLine;

namespace mmm.Options;

[Verb("initialize", aliases: new[] { "init" }, HelpText = "Initialize database")]
internal sealed class InitializeOptions
{
    [Option('f', "force", Required = false, HelpText = "Overwrite existing database")]
    public bool Force { get; set; }
}
