namespace Remouse.Models
{
    public class PlayerSessionData
    {
        public int? playerEntityId;
        public PlayerState state;
    }
    
    public enum PlayerState 
    {
        LoadingMap,
        WaitingWorldState,
        WaitingMapInfo,
        InGame,
        Disconnected
    }
}