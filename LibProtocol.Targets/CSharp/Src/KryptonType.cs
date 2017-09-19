using System;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace Krypton.LibProtocol
{
    public interface IKryptonType
    {
        /// <summary>
        /// Writes the type to an IKryptonCodec
        /// </summary>
        /// <param name="codec"></param>
        void Write(IKryptonCodec codec);

        Task WriteAsync(IKryptonCodec codec);

        /// <summary>
        /// Populates the type with data read from an IKryptonCodec
        /// </summary>
        /// <param name="codec"></param>
        void Read(IKryptonCodec codec);

        Task ReadAsync(IKryptonCodec codec);
    }

    public interface IKryptonType<T> : IKryptonType
    {
    }

    public abstract class KryptonType<T> : IKryptonType where T: IKryptonType<T>, new()
    {
        private static Func<IKryptonType<TK>> GenerateFactory<TK>() where TK: IKryptonType<TK>, new()
        {
            Expression<Func<TK>> expr = () => new TK();
            var newExpr = (NewExpression)expr.Body;
 
            var method = new DynamicMethod(
                name: "lambda", 
                returnType: newExpr.Type,
                parameterTypes: new System.Type[0],
                m: typeof(IKryptonType<TK>).Module,
                skipVisibility: true);
 
            var ilGen = method.GetILGenerator();
            ilGen.Emit(OpCodes.Newobj, newExpr.Constructor);
            ilGen.Emit(OpCodes.Ret);
 
            return (Func<IKryptonType<TK>>)method.CreateDelegate(typeof(Func<IKryptonType<TK>>));
        }

        public static readonly Func<IKryptonType<T>> Create = GenerateFactory<T>();

        /// <summary>
        /// Creates and populates a type from the BufferReader
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static IKryptonType<T> Build(IKryptonCodec s)
        {
            var inst = Create();
            inst.Read(s);
            return inst;
        }

        public abstract void Write(IKryptonCodec codec);

        public abstract void Read(IKryptonCodec codec);
        
        public abstract Task WriteAsync(IKryptonCodec codec);
        
        public abstract Task ReadAsync(IKryptonCodec codec);
    }
}
