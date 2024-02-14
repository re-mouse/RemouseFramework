namespace Infrastructure
{
    public interface IResources
    {
        byte[] GetResource(ResourceType type);
        string GetResourceFilePath(ResourceType type);
        void SaveAs(ResourceType type, byte[] data);
    }
}