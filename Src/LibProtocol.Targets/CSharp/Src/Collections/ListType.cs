using System.Collections.Generic;

namespace Krypton.LibProtocol.Collections
{
    public class ListType<TK> : IKryptonType where TK: IKryptonType, new()
    {
        public List<TK> Value { get; set; }

        public static implicit operator ListType<TK>(List<TK> val)
        {
            return new ListType<TK> { Value = val };
        }
        
        public static implicit operator List<TK>(ListType<TK> val)
        {
            return val.Value;
        }
        
        public void Write(BufferWriter bw)
        {
            bw.WriteUInt16((ushort)Value.Count);
            foreach (var val in Value)
            {
                val.Write(bw);
            }
        }

        public void Consume(BufferReader br)
        {
            Value = new List<TK>();
            var length = br.ReadUInt16();

            for (var i = 0; i < length; i++)
            {
                var x = KryptonType<TK>.Read(br);
                
                Value.Add(x);
            }
        }

        public void Build(BufferReader br)
        {
            Consume(br);
        }
    }
}
