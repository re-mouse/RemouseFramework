using NUnit.Framework;
using System.Collections.Generic;

namespace Remouse.DatabaseLib.Tests
{
    public class TableDataAndSettingsTests
    {
        private Database _database;
        private Settings _exampleSetting;
        private Table<ExampleTableData> _exampleTable;

        [SetUp]
        public void Setup()
        {
            _exampleSetting = new ExampleSettings();
            _exampleTable = new Table<ExampleTableData> { rows = new List<ExampleTableData> { new ExampleTableData { Id = "1" } } };

            var settings = new List<Settings> { _exampleSetting };
            var tables = new List<Table> { _exampleTable };
            _database = new Database(settings, tables);
        }

        [Test]
        public void GetSettings_ReturnsCorrectSettingsType()
        {
            var retrievedSetting = _database.GetSettings<ExampleSettings>();
            Assert.IsNotNull(retrievedSetting);
            Assert.AreEqual(typeof(ExampleSettings), retrievedSetting.GetType());
        }

        [Test]
        public void GetTableData_ReturnsCorrectTableDataType()
        {
            var retrievedData = _database.GetTableData<ExampleTableData>("1");
            Assert.IsNotNull(retrievedData);
            Assert.AreEqual(typeof(ExampleTableData), retrievedData.GetType());
        }

        private class ExampleSettings : Settings { }
        private class ExampleTableData : TableData { }
    }
}