using System;
using System.Collections.Generic;
using System.Linq;
using Shared.Serialization;

namespace Shared.Online.Commands
{
    public static class NetworkMessageSerializer
    {
        private static Dictionary<Type, ushort> _commandIdsByType = new Dictionary<Type, ushort>();
        private static Dictionary<ushort, Type> _commandTypesById = new Dictionary<ushort, Type>();
        static NetworkMessageSerializer()
        {
            var currentAssembly = typeof(NetworkMessageSerializer).Assembly;
            var commands = currentAssembly
                .GetTypes()
                .Where(t => typeof(NetworkMessage).IsAssignableFrom(t))
                .OrderBy(t => t.FullName, StringComparer.Ordinal).ToList();

            for (ushort i = 0; i < commands.Count; i++)
            {
                _commandIdsByType[commands[i]] = i;
                _commandTypesById[i] = commands[i];
            }
        }

        public static void Serialize(NetworkMessage message, INetworkWriter writer)
        {
            WriteMessageTypeId(message, writer);
            message.Serialize(writer);
        }

        public static NetworkMessage? Deserialize(INetworkReader reader)
        {
            ushort typeId = reader.ReadUShort();

            if (!_commandTypesById.TryGetValue(typeId, out var type))
                return null;

            NetworkMessage? message = Activator.CreateInstance(type) as NetworkMessage;
            
            message?.Deserialize(reader);

            return message;
        }

        private static void WriteMessageTypeId(NetworkMessage message, INetworkWriter writer)
        {
            Type type = message.GetType();
            ushort id = _commandIdsByType[type];
            
            writer.WriteUShort(id);
        }
    }
}