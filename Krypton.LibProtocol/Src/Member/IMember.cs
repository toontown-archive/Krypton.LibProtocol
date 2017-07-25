namespace Krypton.LibProtocol.Member
{
    public interface IMember
    {
        string Name { get; }
        IMemberContainer Parent { get; }
    }
}
