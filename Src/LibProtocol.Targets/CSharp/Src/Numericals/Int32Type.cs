namespace Krypton.LibProtocol.Numericals
{
    public struct Int32Type : IKryptonType
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
        
        public void Write(BufferWriter bw)
        {
            bw.WriteInt32(Value);
        }

        public void Consume(BufferReader br)
        {
            Value = br.ReadInt32();
        }
        
        public void Build(BufferReader br)
        {
            Consume(br);
        }
    }
}
