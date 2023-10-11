using System.Collections.Generic;
using System.Numerics;
using Irehon.Entitys;
using Shared.Math;

namespace Shared.Online.Models
{
    public class SceneChangeInfo
    {
        public Vec3 spawnPosition;
        public string sceneName;
    }

    public struct CharacterInfo
    {
        public Fraction fraction;
        public bool isOnlineOnAnotherServer;
        public ulong steamId;
        public string name;
        public int inventoryId;
        public int equipmentId;
        public int serverId;
        public List<PersonalChestInfo> personalChests;
        public string location;
        public Vec3 position;
        public int health;
        public SceneChangeInfo sceneChangeInfo;
        public Vector3 spawnPoint;
        public string spawnSceneName;
        public bool isSpawnPointChanged;
    }
}