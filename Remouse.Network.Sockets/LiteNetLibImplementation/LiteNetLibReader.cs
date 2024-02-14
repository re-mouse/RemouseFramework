using System;
using LiteNetLib;
using Remouse.Serialization;

namespace Remouse.Network.Sockets
{
    public class LiteNetLibReader : IBytesReader
    {
        private NetPacketReader _reader;

        public LiteNetLibReader(NetPacketReader reader)
        {
            _reader = reader;
        }

        public Span<byte> RawAvailable { get => new Span<byte>(_reader.RawData, _reader.Position, _reader.AvailableBytes); }

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
        
        public byte[] ReadByteArray()
        {
            ushort length = ReadUShort();
            return _reader.GetArray<byte>(length);
        }

        public string ReadString()
        {
            return _reader.GetString();
        }
    }
}