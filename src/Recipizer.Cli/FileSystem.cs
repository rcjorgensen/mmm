namespace Recipizer.Cli;

internal sealed class FileSystem
{
    public void Delete(string path)
    {
        File.Delete(path);
    }

    public bool Exists(string path)
    {
        return File.Exists(path);
    }

    public void Create(string path)
    {
        File.Create(path);
    }

    internal Task<string> ReadAllTextAsync(string path)
    {
        return File.ReadAllTextAsync(path);
    }
}
