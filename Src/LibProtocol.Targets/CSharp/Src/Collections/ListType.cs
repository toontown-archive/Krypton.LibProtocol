using System.Collections.Generic;

namespace Krypton.LibProtocol.Collections
{
    public class ListType<TK> : List<TK>, IKryptonType where TK: IKryptonType, new()
    {
        public void Write(BufferWriter bw)
        {
            bw.WriteUInt16((ushort)Count);
            foreach (var val in this)
            {
                val.Write(bw);
            }
        }

        public void Consume(BufferReader br)
        {
            Clear();
            var length = br.ReadUInt16();

            for (var i = 0; i < length; i++)
            {
                var x = KryptonType<TK>.Read(br);
                
                Add(x);
            }
        }

        public void Build(BufferReader br)
        {
            Consume(br);
        }
    }
}
