namespace Remouse.Database
{
    public interface IDatabaseSerializer
    {
        byte[] Serialize(IDatabase database);
        IDatabase Deserialize(byte[] json);
    }
}