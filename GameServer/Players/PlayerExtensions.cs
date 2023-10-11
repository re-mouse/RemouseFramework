using System;
using Shared.GameSimulation;
using Shared.GameSimulation.ECS.Components;
using Shared.Math;
using Shared.Online.Models;

namespace GameServer.Players
{
    public static class PlayerExtensions
    {
        public static Vec2 GetPosition(this PlayerData playerData)
        {
            return playerData.GetTransform().position;
        }

        public static Transform GetTransform(this PlayerData playerData)
        {
            throw new NotImplementedException();
            var simulation = new GameSimulation();

            var world = simulation.World;

            var pool = world.GetPool<Transform>();

            return pool.Get(playerData.sessionData.playerEntityId.Value);
        }

        public static bool IsSpawnedInWorld(this PlayerData playerData)
        {
            return playerData.sessionData.playerEntityId != null;
        }
    }
}