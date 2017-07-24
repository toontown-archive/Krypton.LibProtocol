namespace Krypton.LibProtocol.Collections
{
    public struct BufferType : IKryptonType
    {
        public byte[] Value;

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
        
        public void Write(BufferWriter bw)
        {
            bw.WriteBytes(Value);
        }

        public void Consume(BufferReader br)
        {
            Value = br.Bytes;
            br.SkipBytes(Value.Length);
        }
        
        public void Build(BufferReader br)
        {
            Consume(br);
        }
    }
}
