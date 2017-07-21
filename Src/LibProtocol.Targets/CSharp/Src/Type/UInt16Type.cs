namespace Krypton.LibProtocol.Type
{
    public class UInt16Type : KryptonType<UInt16Type>
    {
        public ushort Value { get; set; }

        public static implicit operator UInt16Type(ushort val)
        {
            return new UInt16Type { Value = val };
        }
        
        public static implicit operator ushort(UInt16Type val)
        {
            return val.Value;
        }
        
        public override void Write(BufferWriter bw)
        {
            bw.WriteUInt16(Value);
        }

        public override void Consume(BufferReader br)
        {
            Value = br.ReadUInt16();
        }
        
        public override void Build(BufferReader br)
        {
            Consume(br);
        }
    }
}
