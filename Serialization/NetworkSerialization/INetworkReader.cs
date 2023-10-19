using System;

namespace Remouse.Serialization
{
    public interface INetworkReader
    {
        Span<byte> RawAvailable { get; }
        int ReadInt();
        uint ReadUInt();
        float ReadFloat();
        double ReadDouble();
        short ReadShort();
        ushort ReadUShort();
        long ReadLong();
        ulong ReadULong();
        bool ReadBool();
        byte ReadByte();
        byte[] ReadByteArray();
        string ReadString();
    }
}