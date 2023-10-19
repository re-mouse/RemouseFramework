using System;

namespace Remouse.Shared.Serialization
{
    public interface INetworkWriter
    {
        ReadOnlySpan<byte> GetBytes();
        void Clear();
        void WriteInt(int value);
        void WriteUInt(uint value);
        void WriteFloat(float value);
        void WriteUShort(ushort value);
        void WriteLong(long value);
        void WriteUlong(ulong value);
        void WriteShort(short value);
        void WriteDouble(double value);
        void WriteChar(char value);
        void WriteBool(bool value);
        void WriteByte(byte value);
        void WriteByteArray(byte[] byteArray);
        void WriteString(string value);
    }
}