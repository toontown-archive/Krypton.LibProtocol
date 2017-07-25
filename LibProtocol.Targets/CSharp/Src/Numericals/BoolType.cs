using System.IO;

namespace Krypton.LibProtocol.Numericals
{
    public struct BoolType : IKryptonType
    {
        public static implicit operator BoolType(bool val)
        {
            return new BoolType { Value = val };
        }

        public static implicit operator bool(BoolType val)
        {
            return val.Value;
        }

        public bool Value;

        public void Write(BinaryWriter bw)
        {
            bw.Write(Value);
        }

        public void Read(BinaryReader br)
        {
            Value = br.ReadBoolean();
        }
    }
}
