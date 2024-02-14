namespace Remouse.UnityEngine.SimulationRender
{
    public class EntityRenderAssetKeys
    {
        public static string GetPrefix()
        {
            return "Entity/";
        }
        
        public static string GetKey(string typeId)
        {
            return $"{GetPrefix()}{typeId}";
        }
    }
}