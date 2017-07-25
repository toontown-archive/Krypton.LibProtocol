using System;
using System.IO;
using System.Linq.Expressions;
using System.Reflection.Emit;

namespace Krypton.LibProtocol
{
    public interface IKryptonType
    {
        /// <summary>
        /// Writes the type to a BufferWriter
        /// </summary>
        /// <param name="bw"></param>
        void Write(BinaryWriter bw);

        /// <summary>
        /// Populates the type with data read from the BufferReader
        /// </summary>
        /// <param name="br"></param>
        void Read(BinaryReader br);
    }

    public abstract class KryptonType<T> : IKryptonType where T: IKryptonType, new()
    {
        private static Func<TK> GenerateFactory<TK>() where TK: IKryptonType, new()
        {
            Expression<Func<TK>> expr = () => new TK();
            var newExpr = (NewExpression)expr.Body;
 
            var method = new DynamicMethod(
                name: "lambda", 
                returnType: newExpr.Type,
                parameterTypes: new System.Type[0],
                m: typeof(KryptonType<>).Module,
                skipVisibility: true);
 
            var ilGen = method.GetILGenerator();
            ilGen.Emit(OpCodes.Newobj, newExpr.Constructor);
            ilGen.Emit(OpCodes.Ret);
 
            return (Func<TK>)method.CreateDelegate(typeof(Func<TK>));
        }
        
        public static readonly Func<T> Create = GenerateFactory<T>();

        /// <summary>
        /// Creates and populates a type from the BufferReader
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static T Build(Stream s)
        {
            var inst = Create();
            using (var br = new BinaryReader(s))
            {
                inst.Read(br);
            }
            return inst;
        }

        public static void Pack(IKryptonType type, Stream s)
        {
            using (var bw = new BinaryWriter(s))
            {
                type.Write(bw);
            }
        }

        public abstract void Write(BinaryWriter bw);

        public abstract void Read(BinaryReader br);
    }
}
