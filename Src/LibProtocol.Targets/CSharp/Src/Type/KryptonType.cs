using System;
using System.Linq.Expressions;
using System.Reflection.Emit;

namespace Krypton.LibProtocol.Type
{
    public abstract class KryptonType<T> where T: KryptonType<T>, new()
    {
        private static Func<TK> GenerateFactory<TK>() where TK: KryptonType<T>, new()
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
            var temp = ilGen.DeclareLocal(newExpr.Type);
            ilGen.Emit(OpCodes.Ldloca, temp);
            ilGen.Emit(OpCodes.Initobj, newExpr.Type);
            ilGen.Emit(OpCodes.Ldloc, temp);
            ilGen.Emit(OpCodes.Ret);
 
            return (Func<TK>)method.CreateDelegate(typeof(Func<TK>));
        }
        
        public static readonly Func<T> CreateInstance = GenerateFactory<T>();
        
        /// <summary>
        /// Writes the type to a BufferWriter
        /// </summary>
        /// <param name="bw"></param>
        public abstract void Write(BufferWriter bw);

        /// <summary>
        /// Populates the type with data read from the BufferReader
        /// </summary>
        /// <param name="br"></param>
        public abstract void Consume(BufferReader br);
        
        /// <summary>
        /// Creates and populates a type from the BufferReader
        /// </summary>
        /// <param name="br"></param>
        /// <returns></returns>
        public static T Read(BufferReader br)
        {
            var inst = CreateInstance();
            inst.Consume(br);
            return inst;
        }
    }
}