namespace Krypton.LibProtocol.Type
{
    public class Int32Type : KryptonType<Int32Type>
    {
        public int Value { get; set; }

        public static implicit operator Int32Type(int val)
        {
            return new Int32Type { Value = val };
        }
        
        public static implicit operator int(Int32Type val)
        {
            return val.Value;
        }
        
        public override void Write(BufferWriter bw)
        {
            bw.WriteInt32(Value);
        }

        public override void Consume(BufferReader br)
        {
            Value = br.ReadInt32();
        }
    }
}
