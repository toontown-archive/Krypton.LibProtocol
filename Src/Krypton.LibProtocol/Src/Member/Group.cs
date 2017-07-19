namespace Krypton.LibProtocol.Member
{
    public class Group : IMember
    {
        public int Id { get; }
        
        public string Name { get; }
        public IMemberContainer Parent { get; }

        public Group(string name, int id, IMemberContainer parent)
        {
            Name = name;
            Id = id;
            Parent = parent;
        }
    }
}
