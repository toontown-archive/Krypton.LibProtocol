using System.IO;

namespace Krypton.LibProtocol.Numericals
{
    public struct Int32Type : IKryptonType
    {
        public static implicit operator Int32Type(int val)
        {
            return new Int32Type { Value = val };
        }
        
        public static implicit operator int(Int32Type val)
        {
            return val.Value;
        }

        public int Value;
        
        public void Write(BinaryWriter bw)
        {
            bw.Write(Value);
        }

        public void Read(BinaryReader br)
        {
            Value = br.ReadInt32();
        }
    }
}
