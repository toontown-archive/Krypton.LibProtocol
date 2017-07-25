using System.Collections.Generic;
using System.IO;

namespace Krypton.LibProtocol.Collections
{
    public class ListType<TK> : List<TK>, IKryptonType where TK: IKryptonType, new()
    {
        public void Write(BinaryWriter bw)
        {
            bw.Write((ushort)Count);
            foreach (var val in this)
            {
                val.Write(bw);
            }
        }

        public void Read(BinaryReader br)
        {
            Clear();
            var length = br.ReadUInt16();

            for (var i = 0; i < length; i++)
            {
                var x = KryptonType<TK>.Create();
                x.Read(br);
                Add(x);
            }
        }
    }
}
