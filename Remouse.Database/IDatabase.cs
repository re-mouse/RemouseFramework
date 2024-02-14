namespace Remouse.Database
{
    public interface IDatabase
    {
        T GetSetting<T>() where T : Settings;
        Table<T> GetTable<T>() where T : TableData;
        T GetTableData<T>(string id) where T : TableData;
        Settings[] GetSettings();
        Table[] GetTables();
    }
}