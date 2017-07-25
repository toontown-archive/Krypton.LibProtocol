namespace Krypton.LibProtocol
{
    public interface IFileResolver
    {
        bool TryResolve(string path, out string result);
    }
}
