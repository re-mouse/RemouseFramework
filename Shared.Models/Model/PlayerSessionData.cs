namespace Remouse.Shared.Models
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
        InGame,
        Disconnected
    }
}