namespace Krypton.LibProtocol.Member.Type
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
    
    public class PrimitiveTypeReference : TypeReference
    {
        public Primitive Type { get; internal set; }

        public override string Name
        {
            get => $"Krypton{Type}";
            internal set { }
        }
    }
}