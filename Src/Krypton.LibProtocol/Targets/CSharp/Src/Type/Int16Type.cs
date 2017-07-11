namespace Krypton.LibProtocol.Type
{
    public class Int16Type : KryptonType<Int16Type>
    {
        public short Value { get; set; }

        public static implicit operator Int16Type(short val)
        {
            return new Int16Type { Value = val };
        }
        
        public static implicit operator short(Int16Type val)
        {
            return val.Value;
        }
        
        public override void Write(BufferWriter bw)
        {
            bw.WriteInt16(Value);
        }

        public override void Consume(BufferReader br)
        {
            Value = br.ReadInt16();
        }
    }
}
