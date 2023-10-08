using CommandLine;

namespace Recipizer.Cli.Options;

[Verb("list")]
internal sealed class ListOptions
{
    [Option('i', "ingredients", SetName = "ingredients")]
    public bool Ingredients { get; set; }

    [Option('n', "inventory", SetName = "inventory")]
    public bool Inventory { get; set; }

    [Option('m', "missing", SetName = "missing")]
    public bool Missing { get; set; }
}
