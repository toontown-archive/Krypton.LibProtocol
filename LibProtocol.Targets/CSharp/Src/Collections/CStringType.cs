using System.IO;

namespace Krypton.LibProtocol.Collections
{
    public struct CStringType : IKryptonType
    {
        public static implicit operator CStringType(string val)
        {
            return new CStringType { Value = val };
        }
        
        public static implicit operator string(CStringType val)
        {
            return val.Value;
        }

        public string Value;
        
        public void Write(BinaryWriter bw)
        {
            bw.Write(Value.ToCharArray());
            bw.Write('\0');
        }

        public void Read(BinaryReader br)
        {
            Value = "";
            
            var c = br.ReadChar();
            while (c != '\0')
            {
                Value += c;
                c = br.ReadChar();
            }
        }
    }
}
