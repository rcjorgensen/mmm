namespace Recipizer.Cli;

internal sealed class Configuration
{
    public string? DatabaseFilePath { get; set; }
    public string? SchemaFilePath { get; set; }
    public string? DataFilePath { get; set; }
}
