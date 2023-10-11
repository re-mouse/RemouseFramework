using System;
using Shared.Online.Models;
using Shared.Serialization;

namespace Shared.Online.Commands.Client
{
    public abstract class InputNetworkMessage : NetworkMessage
    {
        public long simulationTick;
        public PlayerSessionData PlayerData { get; private set; }
        
        public void SetPlayer(PlayerSessionData playerData)
        {
            PlayerData = playerData ?? throw new ArgumentException();
        }

        public override void Deserialize(INetworkReader reader)
        {
            simulationTick = reader.ReadLong();
        }

        public override void Serialize(INetworkWriter writer)
        {
            writer.WriteLong(simulationTick);
        }
    }
}