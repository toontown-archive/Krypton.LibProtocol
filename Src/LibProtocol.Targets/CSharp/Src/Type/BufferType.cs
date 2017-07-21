namespace Krypton.LibProtocol.Type
{
    public class BufferType : KryptonType<BufferType>
    {
        public byte[] Value { get; set; }

        #region BufferWriter casts
        
        public static implicit operator BufferType(BufferWriter val)
        {
            return new BufferType { Value = val.Bytes };
        }
        
        public static implicit operator BufferWriter(BufferType val)
        {
            return new BufferWriter(val.Value);
        }
        
        #endregion
        
        #region BufferReader casts
        
        public static implicit operator BufferType(BufferReader val)
        {
            return new BufferType { Value = val.Bytes };
        }
        
        public static implicit operator BufferReader(BufferType val)
        {
            return new BufferReader(val.Value);
        }
        
        #endregion
        
        #region byte[] casts
        
        public static implicit operator BufferType(byte[] val)
        {
            return new BufferType { Value = val };
        }
        
        public static implicit operator byte[](BufferType val)
        {
            return val.Value;
        }
        
        #endregion
        
        public override void Write(BufferWriter bw)
        {
            bw.WriteBytes(Value);
        }

        public override void Consume(BufferReader br)
        {
            Value = br.Bytes;
            br.SkipBytes(Value.Length);
        }
        
        public override void Build(BufferReader br)
        {
            Consume(br);
        }
    }
}
