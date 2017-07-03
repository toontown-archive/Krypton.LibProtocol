namespace Krypton.LibProtocol.Member
{
    public enum Primitive
    {
        Byte,
        Bool,
        Int8,
        UInt8,
        Int16,
        UInt16,
        Int32,
        UInt32,
        Int64,
        UInt64,
        String,
        CString,
        Buffer
    }

    public class DataStatement : PacketStatement
    {
        public Primitive Type { get; }
        public string Name { get; }

        internal DataStatement(Primitive type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}
