using System;
using System.Collections.Generic;

namespace Krypton.LibProtocol.Type
{
    public class ListType<TK> : KryptonType<ListType<TK>> where TK: KryptonType<TK>, new() 
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
        
        public override void Write(BufferWriter bw)
        {
            bw.WriteUInt16((ushort)Value.Count);
            foreach (var val in Value)
            {
                val.Write(bw);
            }
        }

        public override void Consume(BufferReader br)
        {
            Value = new List<TK>();
            var length = br.ReadUInt16();

            for (var i = 0; i < length; i++)
            {
                var x = KryptonType<TK>.Read(br);
                
                Value.Add(x);
            }
        }

        public override void Build(BufferReader br)
        {
            Consume(br);
        }
    }
}
