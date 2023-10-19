using LiteNetLib;
using Remouse.Shared.Serialization;

namespace Remouse.GameServer.ServerTransport.Implementations
{
    public class LiteNetLibReader : INetworkReader
    {
        private NetPacketReader _reader;

        public LiteNetLibReader(NetPacketReader reader)
        {
            _reader = reader;
        }

        public int ReadInt()
        {
            return _reader.GetInt();
        }

        public uint ReadUInt()
        {
            return _reader.GetUInt();
        }

        public float ReadFloat()
        {
            return _reader.GetFloat();
        }

        public double ReadDouble()
        {
            return _reader.GetDouble();
        }

        public short ReadShort()
        {
            return _reader.GetShort();
        }

        public ushort ReadUShort()
        {
            return _reader.GetUShort();
        }

        public long ReadLong()
        {
            return _reader.GetLong();
        }

        public ulong ReadULong()
        {
            return _reader.GetULong();
        }

        public bool ReadBool()
        {
            return _reader.GetBool();
        }

        public byte ReadByte()
        {
            return _reader.GetByte();
        }

        public string ReadString()
        {
            return _reader.GetString();
        }
    }
}