using CommandLine;

namespace Recipizer.Cli.Options;

[Verb("init")]
internal sealed class InitOptions
{
    [Option('f', "force", Required = false)]
    public bool Force { get; set; }
}
