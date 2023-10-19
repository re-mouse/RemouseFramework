namespace Remouse.Models
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
            steamId = authInfo.id;
            isAuthorized = false;
            character = new CharacterInfo();
            this.authInfo = authInfo;

            isSpawnPointChanged = false;
            sceneToChange = null;
        }
    }
}