using System.Collections.Generic;
using NUnit.Framework;
using Remouse.Core.Configs;

namespace Remouse.DatabaseLib.Tests
{
    public class TableDataLinkTests
    {
        [Test]
        public void TestLink()
        {
            var tables = new List<Table>();
            tables.Add(new Table<TestData>() 
                {
                    rows = new List<TestData>() 
                    {
                        new TestData()
                        {
                            id = "Test", x = 12
                        }
                    } 
                }
            );
            tables.Add(new Table<TestDataWithLink>() 
                {
                    rows = new List<TestDataWithLink>() 
                    {
                        new TestDataWithLink()
                        {
                            id = "TestLink", data = new TableDataLink<TestData>()
                            {
                                Id = "Test"
                            } 
                        }
                    } 
                }
            );

            var database = new Database(new List<Settings>(), tables);

            var dataWithLink = database.GetTableData<TestDataWithLink>("TestLink");
            var data = dataWithLink.data.GetTableData(database);
            
            Assert.AreEqual(12, data.x);
            Assert.AreEqual("Test", data.id);
        }

        private class TestData : TableData
        {
            public int x;
        }

        private class TestDataWithLink : TableData
        {
            public TableDataLink<TestData> data;
        }
    }
}