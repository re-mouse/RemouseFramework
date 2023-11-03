using NUnit.Framework;

namespace Remouse.DatabaseLib.Tests
{

    public class DatabaseSerializerTests
    {
        [Test]
        public void SerializeDeserialize_EmptyDatabase_ReturnsEquivalentDatabase()
        {
            DatabaseBuilder builder = new DatabaseBuilder();
            Database originalDb = builder.Build();

            string serializedDb = new DatabaseJsonSerializer().SerializeToJson(originalDb);
            Database deserializedDb = new DatabaseJsonSerializer().DeserializeFromJson(serializedDb);

            // Here, you might want to add more detailed assertions depending on what constitutes two databases being "equivalent"
            Assert.IsNotNull(deserializedDb);
        }

        [Test]
        public void SerializeDeserialize_DatabaseWithSettings_ReturnsEquivalentDatabase()
        {
            DatabaseBuilder builder = new DatabaseBuilder();
            builder.BindSettings(new SampleSettings { Value = "TestValue" });
            Database originalDb = builder.Build();

            string serializedDbJson = new DatabaseJsonSerializer().SerializeToJson(originalDb);
            TestContext.WriteLine($"Serialized Database: {serializedDbJson}"); // Logging the serialized JSON

            Database deserializedDb = new DatabaseJsonSerializer().DeserializeFromJson(serializedDbJson);

            Assert.IsNotNull(deserializedDb);
            Assert.AreEqual("TestValue", deserializedDb.GetSetting<SampleSettings>().Value);
        }

        [Test]
        public void SerializeDeserialize_DatabaseWithTable_ReturnsEquivalentDatabase()
        {
            DatabaseBuilder builder = new DatabaseBuilder();
            Table<SampleTableData> table = new Table<SampleTableData>();
            table.rows.Add(new SampleTableData { id = "1", Name = "Row1" });
            builder.BindTable(table);
            Database originalDb = builder.Build();

            string serializedDb = new DatabaseJsonSerializer().SerializeToJson(originalDb);
            TestContext.WriteLine($"Serialized Database: {serializedDb}"); // Logging the serialized JSON

            Database deserializedDb = new DatabaseJsonSerializer().DeserializeFromJson(serializedDb);

            Assert.IsNotNull(deserializedDb);
            Assert.AreEqual("Row1", deserializedDb.GetTableData<SampleTableData>("1").Name);
        }

        [Test]
        public void SerializeDeserialize_DatabaseWithSettingsAndTable_ReturnsEquivalentDatabase()
        {
            DatabaseBuilder builder = new DatabaseBuilder();
            builder.BindSettings(new SampleSettings { Value = "TestValue" });

            Table<SampleTableData> table = new Table<SampleTableData>();
            table.rows.Add(new SampleTableData { id = "1", Name = "Row1" });
            builder.BindTable(table);
            Database originalDb = builder.Build();

            string serializedDb = new DatabaseJsonSerializer().SerializeToJson(originalDb);
            Database deserializedDb = new DatabaseJsonSerializer().DeserializeFromJson(serializedDb);

            Assert.IsNotNull(deserializedDb);
            Assert.AreEqual("TestValue", deserializedDb.GetSetting<SampleSettings>().Value);
            Assert.AreEqual("Row1", deserializedDb.GetTableData<SampleTableData>("1").Name);
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