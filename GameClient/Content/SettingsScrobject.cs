using Remouse.Shared.Content;

namespace GameClient.Content
{
    public class SettingsScrobject<T> : ScriptableObject where T : Settings
    {
        public T settings;
    }
}