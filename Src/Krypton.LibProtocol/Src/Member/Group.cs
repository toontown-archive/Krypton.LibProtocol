using Krypton.LibProtocol.Member.Layer;

namespace Krypton.LibProtocol.Member
{
    public class Group : IMember
    {
        public string Name { get; internal set; }
        public int Id { get; internal set; }
        
        public IMemberContainer Parent { get; internal set; }
    }
}