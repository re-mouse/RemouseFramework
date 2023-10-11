namespace GameClient.Content
{
    public class SettingsScrobject<T> : ScriptableObject where T : Shared.Content.Settings
    {
        public T settings;
    }
}