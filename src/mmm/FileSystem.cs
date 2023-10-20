namespace mmm;

internal sealed class FileSystem : IFileSystem
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

    public Task<string> ReadAllText(string path)
    {
        return File.ReadAllTextAsync(path);
    }
}
