namespace Krypton.LibProtocol.Member.Layer
{
    public interface IMember
    {
        string Name { get; }
        IMemberContainer Parent { get; }
    }
}
