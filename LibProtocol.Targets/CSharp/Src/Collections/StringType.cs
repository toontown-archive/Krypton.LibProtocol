using System.IO;

namespace Krypton.LibProtocol.Collections
{
    public struct StringType : IKryptonType
    {
        public static implicit operator StringType(string val)
        {
            return new StringType { Value = val };
        }
        
        public static implicit operator string(StringType val)
        {
            return val.Value;
        }

        public string Value;
        
        public void Write(BinaryWriter bw)
        {
            bw.Write((ushort)Value.Length);
            bw.Write(Value.ToCharArray());
        }

        public void Read(BinaryReader br)
        {
            var length = br.ReadUInt16();
            Value = new string(br.ReadChars(length));
        }
    }
}
