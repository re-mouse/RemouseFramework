using NUnit.Framework;
using System;

namespace Remouse.DatabaseLib.Tests
{
    public class TableTests
    {
        private Table<SampleTableData> _sampleTable;
        
        [SetUp]
        public void Setup()
        { 
            _sampleTable = new Table<SampleTableData>();
            _sampleTable.rows.Add(new SampleTableData { Id = "1", Name = "Row1" });
            _sampleTable.rows.Add(new SampleTableData { Id = "2", Name = "Row2" });
        }

        [Test]
        public void Table_Initialization_ValidData()
        {
            _sampleTable.ValidateRows();
            var data = _sampleTable.GetData("1");
            Assert.AreEqual("Row1", data.Name);
        }

        [Test]
        public void Table_Initialization_DuplicateIds()
        {
            _sampleTable.rows.Add(new SampleTableData { Id = "1", Name = "DuplicateRow" });
            Assert.Throws<AggregateException>(() => _sampleTable.ValidateRows());
        }

        [Test]
        public void Table_Initialization_NullId()
        {
            _sampleTable.rows.Add(new SampleTableData { Id = null, Name = "NullIdRow" });
            Assert.Throws<NullReferenceException>(() => _sampleTable.ValidateRows());
        }

        [Test]
        public void Table_Initialization_NullRow()
        {
            _sampleTable.rows.Add(null);
            Assert.Throws<NullReferenceException>(() => _sampleTable.ValidateRows());
        }

        public class SampleTableData : TableData
        {
            public string Name { get; set; }
        }
    }
}