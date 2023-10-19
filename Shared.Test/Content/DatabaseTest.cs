using NUnit.Framework;
using Remouse.Database;
using System;

namespace Remouse.Database.Tests
{
    public class DatabaseTests
    {
        private SampleSettings _sampleSettings;
        private Table<SampleTableData> _sampleTable;
        
        [SetUp]
        public void Setup()
        {
            _sampleSettings = new SampleSettings { Value = "TestValue" };
            
            _sampleTable = new Table<SampleTableData>();
            _sampleTable.rows.Add(new SampleTableData { Id = "1", Name = "Row1" });
            _sampleTable.rows.Add(new SampleTableData { Id = "2", Name = "Row2" });
        }

        [Test]
        public void SerializeDeserialize_EmptyDatabase_ReturnsEquivalentDatabase()
        {
            DatabaseBuilder builder = new DatabaseBuilder();
            Database originalDb = builder.Build();

            string serializedDb = DatabaseSerializer.SerializeDatabase(originalDb);
            TestContext.WriteLine($"Serialized Database: {serializedDb}");

            Database deserializedDb = DatabaseSerializer.DeserializeDatabase(serializedDb);
            Assert.IsNotNull(deserializedDb);
        }

        [Test]
        public void SerializeDeserialize_MultipleTablesSettings_ReturnsEquivalentDatabase()
        {
            DatabaseBuilder builder = new DatabaseBuilder();
            builder.BindTable(_sampleTable);
            builder.BindSettings(_sampleSettings);
            Database originalDb = builder.Build();

            string serializedDb = DatabaseSerializer.SerializeDatabase(originalDb);
            TestContext.WriteLine($"Serialized Database: {serializedDb}");

            Database deserializedDb = DatabaseSerializer.DeserializeDatabase(serializedDb);
            Assert.IsNotNull(deserializedDb);
            Assert.AreEqual("TestValue", deserializedDb.GetSettings<SampleSettings>().Value);
            Assert.AreEqual("Row1", deserializedDb.GetTableData<SampleTableData>("1").Name);
        }

        [Test]
        public void Deserialize_InvalidJson_ThrowsException()
        {
            string invalidJson = "{\"Invalid\":\"Data\"}";
            Assert.Throws<Exception>(() => DatabaseSerializer.DeserializeDatabase(invalidJson));
        }

        public class SampleSettings : Settings
        {
            public string Value { get; set; }
        }

        public class SampleTableData : TableData
        {
            public string Name { get; set; }
        }
    }
}
