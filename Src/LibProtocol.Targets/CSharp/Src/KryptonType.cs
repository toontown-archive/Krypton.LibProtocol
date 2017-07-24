using System;
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
        void Write(BufferWriter bw);

        /// <summary>
        /// Populates the type with data read from the BufferReader
        /// </summary>
        /// <param name="br"></param>
        void Consume(BufferReader br);

        void Build(BufferReader br);
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
        /// <param name="br"></param>
        /// <returns></returns>
        public static T Read(BufferReader br)
        {
            var inst = Create();
            inst.Build(br);
            return inst;
        }

        public abstract void Build(BufferReader br);
        
        public abstract void Write(BufferWriter bw);

        public abstract void Consume(BufferReader br);
    }
}