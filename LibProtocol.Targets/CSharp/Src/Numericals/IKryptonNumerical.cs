using System;

namespace Krypton.LibProtocol.Numericals
{
    public interface IKryptonNumerical : IComparable, IConvertible, IKryptonType
    {
    }

    public interface IKryptonNumerical<T> 
        : IComparable<T>, IEquatable<T>, IKryptonNumerical
        where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>
    {
    }
}
