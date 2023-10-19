namespace Remouse.Shared.Serialization
{
    public interface INetworkReader
    {
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