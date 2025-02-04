using System;
using System.Text;

namespace Remouse.Serialization
{
    public class BytesReader : IBytesReader
    {
        private int _currentPosition;
        private byte[] _buffer;

        public BytesReader(byte[] buffer)
        {
            _buffer = buffer;
            _currentPosition = 0;
        }

        public Span<byte> RawAvailable { get => new(_buffer, _currentPosition, _buffer.Length - _currentPosition); }

        public int ReadInt()
        {
            int value = FastBitConvert.ToInt32(_buffer, _currentPosition);
            _currentPosition += 4;
            return value;
        }
        
        public uint ReadUInt()
        {
            uint value = FastBitConvert.ToUInt32(_buffer, _currentPosition);
            _currentPosition += 4;
            return value;
        }

        public float ReadFloat()
        {
            float value = FastBitConvert.ToSingle(_buffer, _currentPosition);
            _currentPosition += 4;
            return value;
        }

        public double ReadDouble()
        {
            double value = FastBitConvert.ToDouble(_buffer, _currentPosition);
            _currentPosition += 8;
            return value;
        }

        public short ReadShort()
        {
            short value = FastBitConvert.ToInt16(_buffer, _currentPosition);
            _currentPosition += 2;
            return value;
        }

        public ushort ReadUShort()
        {
            ushort value = FastBitConvert.ToUInt16(_buffer, _currentPosition);
            _currentPosition += 2;
            return value;
        }

        public long ReadLong()
        {
            long value = FastBitConvert.ToInt64(_buffer, _currentPosition);
            _currentPosition += 8;
            return value;
        }

        public ulong ReadULong()
        {
            ulong value = FastBitConvert.ToUInt64(_buffer, _currentPosition);
            _currentPosition += 8;
            return value;
        }

        public bool ReadBool()
        {
            bool value = _buffer[_currentPosition] == 0 ? false : true;
            _currentPosition++;
            return value;
        }

        public byte ReadByte()
        {
            byte value = _buffer[_currentPosition];
            _currentPosition++;
            return value;
        }
        
        public byte[] ReadByteArray()
        {
            ushort length = ReadUShort();
            var array = new byte[length];
            Array.Copy(_buffer, _currentPosition, array, 0, length);
            _currentPosition += length;
            return array;
        }
        
        public string ReadString()
        {
            ushort stringLength = ReadUShort();

            if (stringLength == 0)
                return string.Empty;

            stringLength -= 1;
            string value = Encoding.UTF8.GetString(_buffer, _currentPosition, stringLength);
            _currentPosition += stringLength;
            return value;
        }
    }
}