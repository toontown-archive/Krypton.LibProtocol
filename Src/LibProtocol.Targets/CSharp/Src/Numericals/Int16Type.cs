using System.IO;

namespace Krypton.LibProtocol.Numericals
{
    public struct Int16Type : IKryptonType
    {
        public static implicit operator Int16Type(short val)
        {
            return new Int16Type { Value = val };
        }
        
        public static implicit operator short(Int16Type val)
        {
            return val.Value;
        }

        public short Value;
        
        public void Write(BinaryWriter bw)
        {
            bw.Write(Value);
        }

        public void Read(BinaryReader br)
        {
            Value = br.ReadInt16();
        }
    }
}
