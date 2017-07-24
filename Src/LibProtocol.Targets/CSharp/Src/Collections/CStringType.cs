namespace Krypton.LibProtocol.Collections
{
    public struct CStringType : IKryptonType
    {
        public string Value { get; set; }

        public static implicit operator CStringType(string val)
        {
            return new CStringType { Value = val };
        }
        
        public static implicit operator string(CStringType val)
        {
            return val.Value;
        }
        
        public void Write(BufferWriter bw)
        {
            foreach (var c in Value)
            {
                bw.WriteChar(c);
            }
            
            bw.WriteChar('\0');
        }

        public void Consume(BufferReader br)
        {
            Value = "";
            
            while (true)
            {
                var c = br.ReadChar();
                if (c == '\0')
                {
                    break;
                }

                Value += c;
            }
        }
        
        public void Build(BufferReader br)
        {
            Consume(br);
        }
    }
}
