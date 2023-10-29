using CommandLine;

namespace r7r.Options;

[Verb(
    "import",
    aliases: new[] { "im" },
    HelpText = "Import recipes, ingredients etc. from a JSON file"
)]
internal sealed class ImportOptions
{
    [Value(0, Default = "./data.json", HelpText = "Path to JSON file to import")]
    public required string Path { get; set; }
}
