namespace Krypton.LibProtocol.Type
{
    public class UInt32Type : KryptonType<UInt32Type>
    {
        public uint Value { get; set; }

        public static implicit operator UInt32Type(uint val)
        {
            return new UInt32Type { Value = val };
        }
        
        public static implicit operator uint(UInt32Type val)
        {
            return val.Value;
        }
        
        public override void Write(BufferWriter bw)
        {
            bw.WriteUInt32(Value);
        }

        public override void Consume(BufferReader br)
        {
            Value = br.ReadUInt32();
        }
    }
}
