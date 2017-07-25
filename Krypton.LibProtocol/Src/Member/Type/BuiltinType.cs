namespace Krypton.LibProtocol.Member.Type
{
    public class BuiltinType : IMember
    {        
        public string Name { get; }

        public IMemberContainer Parent { get; }

        public BuiltinType(string name, IMemberContainer parent)
        {
            Name = name;
            Parent = parent;
        }
    }
}
