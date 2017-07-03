namespace Krypton.LibProtocol.Member
{
    public class Message
    {
        public string Name { get; }

        internal Message(string name)
        {
            Name = name;
        }
    }
}
