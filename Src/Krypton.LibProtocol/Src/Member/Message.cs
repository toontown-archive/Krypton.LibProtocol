namespace Krypton.LibProtocol.Member
{
    public class Message
    {
        public string Name { get; }
        
        public int Id { get; internal set; }

        internal Message(string name)
        {
            Name = name;
        }
    }
}
