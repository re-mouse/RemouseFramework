namespace Remouse.Infrastructure
{
    public interface IResources
    {
        byte[] GetResource(ResourceType type);
        void SaveAs(ResourceType type, byte[] data);
    }
}