﻿using System;
using System.Collections.Generic;
using System.Linq;
using Remouse.Shared.Serialization;

namespace Remouse.Shared.Models.Messages
{
    public static class NetworkMessageSerializer
    {
        public static void Serialize(NetworkMessage message, INetworkWriter writer)
        {
            writer.WriteUShort(TypeSerializer<NetworkMessage>.GetTypeId(message));
            message.Serialize(writer);
        }

        public static NetworkMessage? Deserialize(INetworkReader reader)
        {
            ushort typeId = reader.ReadUShort();

            if (!TypeSerializer<NetworkMessage>.TypeExist(typeId))
                return null;

            NetworkMessage? message = TypeSerializer<NetworkMessage>.GetNew(typeId);
            
            message?.Deserialize(reader);

            return message;
        }
    }
}