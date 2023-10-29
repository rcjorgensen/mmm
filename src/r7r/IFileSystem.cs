namespace r7r;

internal interface IFileSystem
{
    void Delete(string path);
    bool Exists(string path);
    void Create(string path);
    Task<string> ReadAllText(string path);
}
