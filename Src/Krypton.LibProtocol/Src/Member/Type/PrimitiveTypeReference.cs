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

    public abstract class PrimitiveTypeReference<T> : TypeReference
    {
        public T Type { get; internal set; }
        
        public override string Name
        {
            get => Type.ToString();
            internal set { }
        }
    }

    public class PrimitiveTypeReference : PrimitiveTypeReference<Primitive>
    {
        public override string TemplateAlias => "primitive_type_reference";
    }
}
