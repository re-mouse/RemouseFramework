namespace Remouse.Database
{
    public interface IDatabaseBuilder
    {
        public void BindTable<T>(Table<T> table, bool overrideIfExist = false) where T : TableData;
        public void BindTable(Table table, bool overrideIfExist);
        public void BindSettings(Settings settings);
        public void Fetch(IDatabase database);
        public IDatabase Build();
    }
}