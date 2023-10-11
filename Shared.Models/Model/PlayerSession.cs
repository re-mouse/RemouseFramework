namespace Shared.Online.Models
{
    public struct PlayerSession
    {
        public ulong steamId;
        public bool isAuthorized;

        public AuthInfo authInfo;

        public CharacterInfo character;

        public bool isSpawnPointChanged;
        public string sceneToChange;

        public PlayerSession(AuthInfo authInfo)
        {
            this.steamId = authInfo.Id;
            this.isAuthorized = false;
            this.character = new CharacterInfo();
            this.authInfo = authInfo;

            this.isSpawnPointChanged = false;
            this.sceneToChange = null;
        }
    }
}