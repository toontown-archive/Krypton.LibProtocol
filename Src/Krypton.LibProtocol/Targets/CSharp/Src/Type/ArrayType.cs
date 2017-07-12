using System.Collections.Generic;

namespace Krypton.LibProtocol.Type
{
    public class ArrayType<TK> : KryptonType<ArrayType<TK>> where TK: KryptonType<TK>, new() 
    {
        public List<TK> Value { get; set; }

        public static implicit operator ArrayType<TK>(List<TK> val)
        {
            return new ArrayType<TK> { Value = val };
        }
        
        public static implicit operator List<TK>(ArrayType<TK> val)
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
                var x = new TK();
                x.Consume(br);
                
                Value.Add(x);
            }
        }
    }
}
