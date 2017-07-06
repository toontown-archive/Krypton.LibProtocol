using System;
using System.Collections.Generic;

namespace Krypton.LibProtocol
{
    /// <summary>
    /// The <see cref="BufferWriter"/> class is used for writing data to a buffer.
    /// </summary>
    public class BufferWriter
    {
        private readonly byte[] _buffer;

        /// <summary>
        /// The amount of bytes in the <see cref="BufferWriter"/>.
        /// </summary>
        public ushort Size { get; private set; }

        /// <summary>
        /// Creates a new <see cref="BufferWriter"/> from an existing byte array
        /// with a fixed size of <see cref="ushort.MaxValue"/>.
        /// </summary>
        /// <param name="data">The byte array.</param>
        public BufferWriter(byte[] data)
        {
            _buffer = new byte[ushort.MaxValue];
            Size = (ushort)data.Length;

            // Copy the data to the buffer.
            Array.Copy(data, _buffer, Size);
        }

        /// <summary>
        /// Creates a new <see cref="BufferWriter"/> with a fixed size.
        /// </summary>
        /// <param name="size">The size of the BufferWriter.</param>
        public BufferWriter(int size)
        {
            _buffer = new byte[size];
            Size = 0;
        }

        /// <summary>
        /// Creates a new <see cref="BufferWriter"/> with a fixed size of <see cref="ushort.MaxValue"/>.
        /// </summary>
        public BufferWriter()
        {
            _buffer = new byte[ushort.MaxValue];
            Size = 0;
        }

        /// <summary>
        /// Writes a single byte representing an Int8 to the <see cref="BufferWriter"/>.
        /// </summary>
        /// <param name="x">The data to write.</param>
        public void WriteInt8(sbyte x)
        {
            _buffer[Size] = (byte)x;
            Size += 1;
        }

        /// <summary>
        /// Writes a single byte representing a UInt8 to the <see cref="BufferWriter"/>.
        /// </summary>
        /// <param name="x">The data to write.</param>
        public void WriteUInt8(byte x)
        {
            _buffer[Size] = x;
            Size += 1;
        }

        /// <summary>
        /// Writes two bytes representing an Int16 to the <see cref="BufferWriter"/>.
        /// </summary>
        /// <param name="x">The data to write.</param>
        public void WriteInt16(short x)
        {
            var bytes = BitConverter.GetBytes(x);
            Array.Copy(bytes, 0, _buffer, Size, 2);
            Size += 2;
        }

        /// <summary>
        /// Writes two bytes representing a UInt16 to the <see cref="BufferWriter"/>.
        /// </summary>
        /// <param name="x">The data to write.</param>
        public void WriteUInt16(ushort x)
        {
            var bytes = BitConverter.GetBytes(x);
            Array.Copy(bytes, 0, _buffer, Size, 2);
            Size += 2;
        }

        /// <summary>
        /// Writes four bytes representing an Int32 to the <see cref="BufferWriter"/>.
        /// </summary>
        /// <param name="x">The data to write.</param>
        public void WriteInt32(int x)
        {
            var bytes = BitConverter.GetBytes(x);
            Array.Copy(bytes, 0, _buffer, Size, 4);
            Size += 4;
        }

        /// <summary>
        /// Writes four bytes representing a UInt32 to the <see cref="BufferWriter"/>.
        /// </summary>
        /// <param name="x">The data to write.</param>
        public void WriteUInt32(uint x)
        {
            var bytes = BitConverter.GetBytes(x);
            Array.Copy(bytes, 0, _buffer, Size, 4);
            Size += 4;
        }

        /// <summary>
        /// Writes eight bytes representing an Int64 to the <see cref="BufferWriter"/>.
        /// </summary>
        /// <param name="x">The data to write.</param>
        public void WriteInt64(long x)
        {
            var bytes = BitConverter.GetBytes(x);
            Array.Copy(bytes, 0, _buffer, Size, 8);
            Size += 8;
        }

        /// <summary>
        /// Writes eight bytes representing a UInt64 to the <see cref="BufferWriter"/>.
        /// </summary>
        /// <param name="x">The data to write.</param>
        public void WriteUInt64(ulong x)
        {
            var bytes = BitConverter.GetBytes(x);
            Array.Copy(bytes, 0, _buffer, Size, 8);
            Size += 8;
        }

        /// <summary>
        /// Writes four bytes representing a Float32 to the <see cref="BufferWriter"/>.
        /// </summary>
        /// <param name="x">The data to write.</param>
        public void WriteFloat32(float x)
        {
            var bytes = BitConverter.GetBytes(x);
            Array.Copy(bytes, 0, _buffer, Size, 4);
            Size += 4;
        }

        /// <summary>
        /// Writes eight bytes representing a Float64 to the <see cref="BufferWriter"/>.
        /// </summary>
        /// <param name="x">The data to write.</param>
        public void WriteFloat64(double x)
        {
            var bytes = BitConverter.GetBytes(x);
            Array.Copy(bytes, 0, _buffer, Size, 8);
            Size += 8;
        }

        /// <summary>
        /// Writes a single byte representing a char to the <see cref="BufferWriter"/>.
        /// </summary>
        /// <param name="x">The data to write.</param>
        public void WriteChar(char x)
        {
            _buffer[Size] = (byte)x;
            Size += 1;
        }

        /// <summary>
        /// Writes two bytes representing the length of the string as a UInt16 as well as
        /// every character in the string to the <see cref="BufferWriter"/>.
        /// </summary>
        /// <param name="x">The data to write.</param>
        public void WriteString(string x)
        {
            WriteUInt16((ushort)x.Length);
            foreach (var c in x)
            {
                WriteChar(c);
            }
        }

        /// <summary>
        /// Writes a single byte representing a boolean to the <see cref="BufferWriter"/>.
        /// </summary>
        /// <param name="x">The data to write.</param>
        public void WriteBool(bool x)
        {
            if (x)
            {
                WriteUInt8(1);
                return;
            }

            WriteUInt8(0);
        }

        /// <summary>
        /// Writes a single byte to the <see cref="BufferWriter"/>.
        /// </summary>
        /// <param name="x">The data to write.</param>
        public void WriteByte(byte x)
        {
            _buffer[Size] = x;
            Size += 1;
        }

        /// <summary>
        /// Writes bytes.Length bytes to the <see cref="BufferWriter"/>.
        /// </summary>
        /// <param name="x">The data to write.</param>
        public void WriteBytes(IEnumerable<byte> x)
        {
            foreach (var b in x)
            {
                _buffer[Size] = b;
                Size += 1;
            }
        }

        /// <summary>
        /// Moves the buffer index up by n.
        /// </summary>
        /// <param name="n">The amount to move the buffer index by.</param>
        public void PadBytes(ushort n)
        {
            Size += n;
        }

        /// <summary>
        /// A copy of all of the bytes in the <see cref="BufferWriter"/>.
        /// </summary>
        public byte[] Bytes
        {
            get
            {
                var data = new byte[Size];
                Array.Copy(_buffer, data, Size);
                return data;
            }
        }
    }
}
