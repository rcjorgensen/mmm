using CommandLine;

namespace Recipizer.Cli.Options;

[Verb("recipes")]
internal sealed class RecipesOptions
{
    [Option('l', "list", SetName = "list")]
    public bool List { get; set; }

    [Option('m', "match", SetName = "list")]
    public string? Match { get; set; }

    [Option('a', "add", SetName = "add")]
    public bool Add { get; set; }

    [Option("name", SetName = "add")]
    public required string Name { get; set; }

    [Option("details", SetName = "add")]
    public string? Details { get; set; }

    [Option('r', "remove", SetName = "remove")]
    public bool Remove { get; set; }

    [Option("id", SetName = "remove")]
    public long Id { get; set; }
}
