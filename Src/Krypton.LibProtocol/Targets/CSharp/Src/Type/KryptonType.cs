namespace Krypton.LibProtocol.Type
{
    public abstract class KryptonType<T> where T: KryptonType<T>, new()
    {
        /// <summary>
        /// Writes the type to a BufferWriter
        /// </summary>
        /// <param name="bw"></param>
        public abstract void Write(BufferWriter bw);

        /// <summary>
        /// Reads the type from a BufferReader
        /// </summary>
        /// <param name="br"></param>
        public abstract void Consume(BufferReader br);
        
        public static T Read(BufferReader br)
        {
            var inst = new T();
            inst.Consume(br);
            return inst;
        }
    }
}