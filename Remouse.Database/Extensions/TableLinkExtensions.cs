namespace Remouse.Database
{
    public static class TableLinkExtensions
    {
        public static T GetTableData<T>(this TableDataLink<T> dataLink, IDatabase database) where T : TableData
        {
            return database.GetTableData<T>(dataLink.Id);
        }
    }
}