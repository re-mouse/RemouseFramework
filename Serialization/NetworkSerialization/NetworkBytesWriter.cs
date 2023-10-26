using System;
using System.Text;

namespace Remouse.Serialization
{
    public class NetworkBytesWriter : INetworkWriter
    {
        private const ushort MaxBufferSize = 1500;

        private int _max8BytesPosition = MaxBufferSize - 8;
        private int _max4BytesPosition = MaxBufferSize - 4;
        private int _max2BytesPosition = MaxBufferSize - 2;
        private int _max1BytesPosition = MaxBufferSize - 1;
        
        private byte[] _buffer;

        private int _currentPosition;

        public NetworkBytesWriter()
        {
            _buffer = new byte[MaxBufferSize];
        }
        
        public NetworkBytesWriter(byte[] data)
        {
            _buffer = data;
            _currentPosition = data.Length;
        }

        public ReadOnlySpan<byte> GetBytes()
        {
            return new ReadOnlySpan<byte>(_buffer, 0, _currentPosition);
        }
        
        public void Clear()
        {
            _currentPosition = 0;
        }

        public void WriteInt(int value)
        {
            if (_currentPosition >= _max4BytesPosition)
            {
                Resize();
            }
            
            FastBitConvert.GetBytes(_buffer, _currentPosition, value);
            _currentPosition += 4;
        }

        public void WriteUInt(uint value)
        {
            if (_currentPosition >= _max4BytesPosition)
            {
                Resize();
            }
            
            FastBitConvert.GetBytes(_buffer, _currentPosition, value);
            _currentPosition += 4;
        }
        
        public void WriteFloat(float value)
        {
            if (_currentPosition >= _max4BytesPosition)
            {
                Resize();
            }
            
            FastBitConvert.GetBytes(_buffer, _currentPosition, value);
            _currentPosition += 4;
        }

        public void WriteUShort(ushort value)
        {
            if (_currentPosition >= _max2BytesPosition)
            {
                Resize();
            }
            
            FastBitConvert.GetBytes(_buffer, _currentPosition, value);
            _currentPosition += 2;
        }

        public void WriteLong(long value)
        {
            if (_currentPosition >= _max8BytesPosition)
            {
                Resize();
            }
            
            FastBitConvert.GetBytes(_buffer, _currentPosition, value);
            _currentPosition += 8;
        }

        public void WriteUlong(ulong value)
        {
            if (_currentPosition >= _max8BytesPosition)
            {
                Resize();
            }
            
            FastBitConvert.GetBytes(_buffer, _currentPosition, value);
            _currentPosition += 8;
        }

        public void WriteShort(short value)
        {
            if (_currentPosition >= _max2BytesPosition)
            {
                Resize();
            }
            
            FastBitConvert.GetBytes(_buffer, _currentPosition, value);
            _currentPosition += 2;
        }

        public void WriteDouble(double value)
        {
            if (_currentPosition >= _max8BytesPosition)
            {
                Resize();
            }
            
            FastBitConvert.GetBytes(_buffer, _currentPosition, value);
            _currentPosition += 8;
        }

        public void WriteChar(char value)
        {
            if (_currentPosition >= _max1BytesPosition)
            {
                Resize();
            }
            
            if (value <= 255)
            {
                _buffer[_currentPosition] = (byte)value;
                _currentPosition++;
            }
            else
            {
                throw new Exception("Character value cannot fit into a single byte");
            }
        }

        public void WriteBool(bool value)
        {
            if (_currentPosition >= _max1BytesPosition)
            {
                Resize();
            }
            
            _buffer[_currentPosition] = value ? (byte)1 : (byte)0;
            _currentPosition++;
        }

        public void WriteByte(byte value)
        {
            if (_currentPosition >= _max1BytesPosition)
            {
                Resize();
            }
            
            _buffer[_currentPosition] = value;
            _currentPosition++;
        }
        
        public void WriteByteArray(byte[] byteArray)
        {
            WriteUShort((ushort)byteArray.Length);
            
            if (_currentPosition >= _buffer.Length - byteArray.Length)
            {
                Resize();
            }
            
            Array.Copy(byteArray, 0, _buffer, _currentPosition, byteArray.Length);
            _currentPosition += byteArray.Length;
        }
        
        public void WriteString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                WriteUShort(0);
                return;
            }
            
            int bytesCount = Encoding.UTF8.GetByteCount(value);

            if (bytesCount > ushort.MaxValue)
                throw new InvalidOperationException($"Message too big, not supported. Message size was {bytesCount}");
            
            WriteUShort((ushort)(bytesCount + 1));
            
            if (_currentPosition + bytesCount >= _buffer.Length)
            {
                Resize();
            }
            
            Encoding.UTF8.GetBytes(value, 0, value.Length, _buffer, _currentPosition);
            _currentPosition += bytesCount;
        }
        
        private void Resize()
        {
            Array.Resize(ref _buffer, _buffer.Length * 2);

            _max8BytesPosition = _buffer.Length - 8;
            _max4BytesPosition = _buffer.Length - 4;
            _max2BytesPosition = _buffer.Length - 2;
            _max1BytesPosition = _buffer.Length - 1;
        }
    }
}