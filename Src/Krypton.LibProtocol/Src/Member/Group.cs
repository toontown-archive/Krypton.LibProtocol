namespace Krypton.LibProtocol.Member
{
    public class Group
    {
        public string Name { get; }
        public int Id { get; internal set; }

        internal Group(string name)
        {
            Name = name;
        }
    }
}