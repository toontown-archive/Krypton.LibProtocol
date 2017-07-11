using System;
using System.Collections.Generic;

namespace Krypton.LibProtocol
{
    /// <summary>
    /// The <see cref="BufferReader"/> class is used for reading data from a buffer.
    /// </summary>
    public class BufferReader
    {
        private readonly byte[] _buffer;
        private ushort _offset;
        
        /// <summary>
        /// Creates a new <see cref="BufferReader"/> with data from an existing byte array. 
        /// </summary>
        /// <param name="data">The byte array.</param>
        public BufferReader(byte[] data) 
        {
            _offset = 0;
            _buffer = data;
        }

        /// <summary>
        /// Creates a new <see cref="BufferReader"/> with data from an existing <see cref="BufferWriter"/>.
        /// </summary>
        /// <param name="bw">The <see cref="BufferWriter"/>.</param>
        public BufferReader(BufferWriter bw)
        {
            _offset = 0;
            _buffer = bw.Bytes;
        }

        /// <summary>
        /// Creates a new <see cref="BufferReader"/> with data from an existing <see cref="BufferReader"/>.
        /// </summary>
        /// <param name="bw">The <see cref="BufferWriter"/>.</param>
        public BufferReader(BufferReader br)
        {
            _offset = 0;
            _buffer = br.Bytes;
        }

        /// <summary>
        /// Reads a single byte from the <see cref="BufferReader"/>.
        /// </summary>
        /// <returns>A byte.</returns>
        public byte ReadByte()
        {
            return _buffer[_offset++];
        }

        /// <summary>
        /// Reads a list of bytes form the <see cref="Buffer"/>
        /// </summary>
        /// <param name="count">The amount of bytes to read</param>
        /// <returns>A list of bytes.</returns>
        public IList<byte> ReadBytes(int count)
        {
            var data = new byte[count];
            Array.Copy(_buffer, _offset, data, 0, data.Length);
            _offset += (ushort)count;
            return data;
        }

        /// <summary>
        /// Reads a single byte from the <see cref="BufferReader"/>.
        /// </summary>
        /// <returns>An Int8 representing the bytes read.</returns>
        public sbyte ReadInt8()
        {
            return (sbyte)_buffer[_offset++];
        }
        
        /// <summary>
        /// Reads a single byte from the <see cref="BufferReader"/>.
        /// </summary>
        /// <returns>A UInt8 representing the bytes read.</returns>
        public byte ReadUInt8()
        {
            return _buffer[_offset++];
        }
        
        /// <summary>
        /// Reads two bytes from the <see cref="BufferReader"/>.
        /// </summary>
        /// <returns>An Int16 representing the two bytes read.</returns>
        public short ReadInt16()
        {
            var i = BitConverter.ToInt16(_buffer, _offset);
            _offset += 2;
            return i;
        }
        
        /// <summary>
        /// Reads two bytes from the <see cref="BufferReader"/>.
        /// </summary>
        /// <returns>A UInt16 representing the two bytes read.</returns>
        public ushort ReadUInt16()
        {
            var i = BitConverter.ToUInt16(_buffer, _offset);
            _offset += 2;
            return i;
        }
        
        /// <summary>
        /// Reads four bytes from the <see cref="BufferReader"/>.
        /// </summary>
        /// <returns>An Int32 representing the four bytes read.</returns>
        public int ReadInt32()
        {
            var i = BitConverter.ToInt32(_buffer, _offset);
            _offset += 4;
            return i;
        }
        
        /// <summary>
        /// Reads four bytes from the <see cref="BufferReader"/>.
        /// </summary>
        /// <returns>A UInt32 representing the four bytes read.</returns>
        public uint ReadUInt32()
        {
            var i = BitConverter.ToUInt32(_buffer, _offset);
            _offset += 4;
            return i;
        }
        
        /// <summary>
        /// Reads eight bytes from the <see cref="BufferReader"/>.
        /// </summary>
        /// <returns>An Int64 representing the eight bytes read.</returns>
        public long ReadInt64()
        {
            var i = BitConverter.ToInt64(_buffer, _offset);
            _offset += 8;
            return i;
        }
        
        /// <summary>
        /// Reads eight bytes from the <see cref="BufferReader"/>.
        /// </summary>
        /// <returns>A UInt64 representing the eight bytes read.</returns>
        public ulong ReadUInt64()
        {
            var i = BitConverter.ToUInt64(_buffer, _offset);
            _offset += 8;
            return i;
        }

        /// <summary>
        /// Reads four bytes from the <see cref="BufferReader"/>
        /// </summary>
        /// <returns>A Float32 representing the four bytes read.</returns>
        public float ReadFloat32()
        {
            var i = BitConverter.ToSingle(_buffer, _offset);
            _offset += 4;
            return i;
        }

        /// <summary>
        /// Reads eight bytes from the <see cref="BufferReader"/>
        /// </summary>
        /// <returns>A Float64 representing the eight bytes read.</returns>
        public double ReadFloat64()
        {
            var i = BitConverter.ToDouble(_buffer, _offset);
            _offset += 8;
            return i;
        }

        /// <summary>
        /// Reads a single byte from the <see cref="BufferReader"/>.
        /// </summary>
        /// <returns>A char representing the single byte read.</returns>
        public char ReadChar()
        {
            return (char)_buffer[_offset++];
        }

        /// <summary>
        /// Reads two + n bytes from the <see cref="BufferReader"/>.
        /// </summary>
        /// <returns>A string representing the n bytes read.</returns>
        public string ReadString()
        {
            var s = string.Empty;

            var size = ReadUInt16();
            for (var i = 0; i < size; i++)
            {
                s += ReadChar();
            }

            return s;
        }

        /// <summary>
        /// Reads a single byte from the <see cref="BufferReader"/>.
        /// </summary>
        /// <returns>A boolean representing the single byte read.</returns>
        public bool ReadBool()
        {
            var x = ReadUInt8();
            return x == 1;
        }

        public void SkipBytes(int n)
        {
            _offset += (ushort)n;
        }

        /// <summary>
        /// Resets the buffer offset to zero.
        /// </summary>
        public void SeekPayload()
        {
            _offset = 0;
        }

        /// <summary>
        /// Gets a copy of the remaining bytes in the <see cref="BufferReader"/>.
        /// </summary>
        public byte[] Bytes
        {
            get
            {
                var data = new byte[_buffer.Length - _offset];
                Array.Copy(_buffer, _offset, data, 0, data.Length);
                return data;
            }
        }

        /// <summary>
        /// Gets the amount of remaining bytes in the <see cref="BufferReader"/>.
        /// </summary>
        public ushort Size => (ushort)(_buffer.Length - _offset);
    }
}
