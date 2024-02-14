using System.Collections.Generic;
using Remouse.Math;

namespace Remouse.Serialization
{
    public static class BytesSerializerExtensions
    {
        public static void Serialize(this DeterminedRandom random, IBytesWriter writer)
        {
            writer.WriteUInt(random.x);
            writer.WriteUInt(random.y);
            writer.WriteUInt(random.z);
            writer.WriteUInt(random.w);
        }
        
        public static void Deserialize(this DeterminedRandom random, IBytesReader reader)
        {
            random.x = reader.ReadUInt();
            random.y = reader.ReadUInt();
            random.z = reader.ReadUInt();
            random.w = reader.ReadUInt();
        }
        
        public static void Serialize(this Vec2 vec2, IBytesWriter writer)
        {
            writer.WriteFloat(vec2.x);
            writer.WriteFloat(vec2.y);
        }
        
        public static void Deserialize(this ref Vec2 vec2, IBytesReader reader)
        {
            vec2.x = reader.ReadFloat();
            vec2.y = reader.ReadFloat();
        }
        
        public static void Serialize(this Vec3 vec3, IBytesWriter writer)
        {
            writer.WriteFloat(vec3.x);
            writer.WriteFloat(vec3.y);
            writer.WriteFloat(vec3.z);
        }
        
        public static void Deserialize(this ref Vec3 vec3, IBytesReader reader)
        {
            vec3.x = reader.ReadFloat();
            vec3.y = reader.ReadFloat();
            vec3.z = reader.ReadFloat();
        }
        
        public static void Serialize(this Vec4 vec4, IBytesWriter writer)
        {
            writer.WriteFloat(vec4.x);
            writer.WriteFloat(vec4.y);
            writer.WriteFloat(vec4.z);
            writer.WriteFloat(vec4.w);
        }
        
        public static void Deserialize(this ref Vec4 vec4, IBytesReader reader)
        {
            vec4.x = reader.ReadFloat();
            vec4.y = reader.ReadFloat();
            vec4.z = reader.ReadFloat();
            vec4.w = reader.ReadFloat();
        }
        
        public static void WriteList<T>(this IBytesWriter writer, List<T> list) where T : IBytesSerializable
        {
            if (list == null)
            {
                writer.WriteInt(0);
                return;
            }
            
            writer.WriteInt(list.Count);

            foreach (T value in list)
            {
                value.Serialize(writer);
            }
        }
        
        public static void ReadList<T>(this IBytesReader reader, ref List<T> list) where T : IBytesSerializable, new()
        {
            if (list == null)
                list = new List<T>();
            
            int length = reader.ReadInt();
            
            list.Clear();
            
            list.Capacity = length;

            for (int i = 0; i < length; i++)
            {
                T value = new T();
                
                value.Deserialize(reader);
                
                list.Add(value);
            }
        }
        
        public static void WriteList(this IBytesWriter writer, List<string> list)
        {
            writer.WriteInt(list.Count);

            foreach (var value in list)
            {
                writer.WriteString(value);
            }
        }
        
        public static void ReadList(this IBytesReader reader, ref List<string> list)
        {
            if (list == null)
                list = new List<string>();

            int length = reader.ReadInt();
            
            list.Clear();

            for (int i = 0; i < length; i++)
            {
                list.Add(reader.ReadString());
            }
        }
        
        public static void WriteList(this IBytesWriter writer, List<int> list)
        {
            writer.WriteInt(list.Count);

            foreach (var value in list)
            {
                writer.WriteInt(value);
            }
        }
        
        public static void ReadList(this IBytesReader reader, ref List<int> list)
        {
            if (list == null)
                list = new List<int>();

            int length = reader.ReadInt();
            
            list.Clear();

            for (int i = 0; i < length; i++)
            {
                list.Add(reader.ReadInt());
            }
        }
        
        public static void WriteList(this IBytesWriter writer, List<uint> list)
        {
            writer.WriteInt(list.Count);

            foreach (var value in list)
            {
                writer.WriteUInt(value);
            }
        }

        public static void ReadList(this IBytesReader reader, ref List<uint> list)
        {
            if (list == null)
                list = new List<uint>();

            int length = reader.ReadInt();

            list.Clear();

            for (int i = 0; i < length; i++)
            {
                list.Add(reader.ReadUInt());
            }
        }

        public static void WriteList(this IBytesWriter writer, List<long> list)
        {
            writer.WriteInt(list.Count);

            foreach (var value in list)
            {
                writer.WriteLong(value);
            }
        }

        public static void ReadList(this IBytesReader reader, ref List<long> list)
        {
            if (list == null)
                list = new List<long>();

            int length = reader.ReadInt();

            list.Clear();

            for (int i = 0; i < length; i++)
            {
                list.Add(reader.ReadLong());
            }
        }

        public static void WriteList(this IBytesWriter writer, List<ulong> list)
        {
            writer.WriteInt(list.Count);

            foreach (var value in list)
            {
                writer.WriteUlong(value);
            }
        }

        public static void ReadList(this IBytesReader reader, ref List<ulong> list)
        {
            if (list == null)
                list = new List<ulong>();

            int length = reader.ReadInt();

            list.Clear();

            for (int i = 0; i < length; i++)
            {
                list.Add(reader.ReadULong());
            }
        }

        public static void WriteList(this IBytesWriter writer, List<ushort> list)
        {
            writer.WriteInt(list.Count);

            foreach (var value in list)
            {
                writer.WriteUShort(value);
            }
        }

        public static void ReadList(this IBytesReader reader, ref List<ushort> list)
        {
            if (list == null)
                list = new List<ushort>();

            int length = reader.ReadInt();

            list.Clear();

            for (int i = 0; i < length; i++)
            {
                list.Add(reader.ReadUShort());
            }
        }

        public static void WriteList(this IBytesWriter writer, List<short> list)
        {
            writer.WriteInt(list.Count);

            foreach (var value in list)
            {
                writer.WriteShort(value);
            }
        }

        public static void ReadList(this IBytesReader reader, ref List<short> list)
        {
            if (list == null)
                list = new List<short>();

            int length = reader.ReadInt();

            list.Clear();

            for (int i = 0; i < length; i++)
            {
                list.Add(reader.ReadShort());
            }
        }

        public static void WriteList(this IBytesWriter writer, List<byte> list)
        {
            writer.WriteInt(list.Count);

            foreach (var value in list)
            {
                writer.WriteByte(value);
            }
        }

        public static void ReadList(this IBytesReader reader, ref List<byte> list)
        {
            if (list == null)
                list = new List<byte>();
            
            int length = reader.ReadInt();

            list.Clear();

            for (int i = 0; i < length; i++)
            {
                list.Add(reader.ReadByte());
            }
        }
        
        public static void WriteList(this IBytesWriter writer, List<Vec2> list)
        {
            writer.WriteInt(list.Count);

            foreach (var value in list)
            {
                value.Serialize(writer);
            }
        }

        public static void ReadList(this IBytesReader reader, ref List<Vec2> list)
        {
            if (list == null)
                list = new List<Vec2>();
            
            int length = reader.ReadInt();

            list.Clear();

            for (int i = 0; i < length; i++)
            {
                var vec2 = new Vec2();
                vec2.Deserialize(reader);
                list.Add(vec2);
            }
        }
    }
}