namespace Shared.Online.Models
{
    public partial class PlayerSessionData
    {
        public int? playerEntityId;
        public PlayerState state;
    }
    
    public enum PlayerState 
    {
        WaitingLoadMap,
        Loaded,
        Spawned
    }
}