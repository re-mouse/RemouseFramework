using Remouse.UnityEngine.Assets;
using Newtonsoft.Json;

namespace Remouse.UnityEngine.Assets
{
    public class Asset
    {
        public string key;
        [JsonIgnore]
        public virtual string prefix { get => DefaultAssetKeys.GetPrefix(); }
    }
}