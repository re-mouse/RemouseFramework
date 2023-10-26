using NUnit.Framework;
using Remouse.Core.Configs;
using System.Collections.Generic;
using Remouse.DatabaseLib.Tables;
using Remouse.Mathlib;
using Remouse.MathLib;

namespace Remouse.DatabaseLib.Tests
{
    [TestFixture]
    public class MapSerializeTests
    {
        [Test]
        public void TestDeserialize()
        {
            Table<MapConfig> mapTable = new Table<MapConfig>();
            
            mapTable.rows.Add(GetMapConfig());
            mapTable.rows.Add(GetStartMapConfig());

            var tables = new List<Table>();
            tables.Add(mapTable);
            
            var database = new Database(new List<Settings>(), tables);

            var json = DatabaseSerializer.SerializeDatabase(database);
            database = DatabaseSerializer.DeserializeDatabase(json);

            var defaultMap = database.GetTableData<MapConfig>("Map");
            Assert.IsNotNull(defaultMap);
            Assert.AreEqual("Map", defaultMap.Id);

            // Проверка для EntityConfig
            var defaultEntity = defaultMap.entities[0];
            Assert.AreEqual("monster", defaultEntity.typeId.Id);
    
            // Проверка для ComponentConfig
            var movementSpeedComponent = defaultEntity.overrideComponents[0];
            Assert.AreEqual("movementSpeed", movementSpeedComponent.name);
            Assert.AreEqual("15", movementSpeedComponent.componentValues["speed"]);
    
            var healthComponent = defaultEntity.overrideComponents[1];
            Assert.AreEqual("health", healthComponent.name);
            Assert.AreEqual("20", healthComponent.componentValues["startHealth"]);
            Assert.AreEqual("50", healthComponent.componentValues["maxHealth"]);
    
            // Проверка для Position и Rotation
            Assert.AreEqual(new Vec3(10.45f, 12f, 123f), defaultEntity.position);
            Assert.AreEqual(new Vec4(0f, 0f, 90f, 1f), defaultEntity.rotation);

            // Проверка для TileConfig
            var defaultTile = defaultMap.tiles[0];
            Assert.AreEqual("grass", defaultTile.typeId.Id);
            Assert.AreEqual(new Vec2Int(10, 12), defaultTile.position);
        }
        
        [Test]
        public void TestSerializeNoExceptions()
        {
            Table<MapConfig> mapTable = new Table<MapConfig>();
            
            mapTable.rows.Add(GetMapConfig());
            mapTable.rows.Add(GetStartMapConfig());

            var tables = new List<Table>();
            tables.Add(mapTable);
            
            var database = new Database(new List<Settings>(), tables);

            var json = DatabaseSerializer.SerializeDatabase(database);
            TestContext.Out.WriteLine(json);
        }

        private static MapConfig GetMapConfig()
        {
            var item = new MapConfig();
            item.Id = "Map";
            item.entities = new EntityConfig[]
            {
                new EntityConfig
                {
                    typeId = new TableDataLink<EntityTypeConfig>() { Id = "monster" },
                    overrideComponents = new ComponentConfig[]
                    {
                        new ComponentConfig
                        {
                            name = "movementSpeed",
                            componentValues = new Dictionary<string, object>
                            {
                                { "speed", 15.ToString() }
                            }
                        },
                        new ComponentConfig
                        {
                            name = "health",
                            componentValues = new Dictionary<string, object>
                            {
                                { "startHealth", 20.ToString() },
                                { "maxHealth", 50.ToString() }
                            }
                        }
                    },
                    position = new Vec3(10.45f, 12f, 123f),
                    rotation = new Vec4(0f, 0f, 90f, 1f)
                },
            };
            item.tiles = new TileConfig[]
            {
                new TileConfig
                {
                    typeId = new TableDataLink<TileTypeConfig>() { Id = "grass" },
                    position = new Vec2Int(10, 12)
                }
            };
            return item;
        }
        
        private static MapConfig GetStartMapConfig()
        {
            var item = new MapConfig();
            item.Id = "Start";
            item.entities = new EntityConfig[]
            {
                new EntityConfig
                {
                    typeId = new TableDataLink<EntityTypeConfig>() { Id = "monster" },
                    overrideComponents = new ComponentConfig[]
                    {
                        new ComponentConfig
                        {
                            name = "movementSpeed",
                            componentValues = new Dictionary<string, object>
                            {
                                { "speed", 15.ToString() }
                            }
                        },
                        new ComponentConfig
                        {
                            name = "health",
                            componentValues = new Dictionary<string, object>
                            {
                                { "startHealth", 20.ToString() },
                                { "maxHealth", 50.ToString() }
                            }
                        }
                    },
                    position = new Vec3(10.45f, 12f, 123f),
                    rotation = new Vec4(0f, 0f, 90f, 1f)
                },
                new EntityConfig()
                {
                    typeId = new TableDataLink<EntityTypeConfig>() { Id = "playerSpawnPoint" },
                    overrideComponents = new ComponentConfig[]
                    {
                        new ComponentConfig
                        {
                            name = "playerCapacity",
                            componentValues = new Dictionary<string, object>
                            {
                                { "capacity", 20 }
                            }
                        },
                        new ComponentConfig
                        {
                            name = "health",
                            componentValues = new Dictionary<string, object>
                            {
                                { "startHealth", 40 },
                                { "maxHealth", 50 }
                            }
                        }
                    },
                    position = new Vec3(102.45f, 132f, 123f),
                    rotation = new Vec4(0f, 0f, 90f, 1f)
                }
            };
            item.tiles = new TileConfig[]
            {
                new TileConfig
                {
                    typeId = new TableDataLink<TileTypeConfig>() { Id = "grass" },
                    position = new Vec2Int(10, 12)
                },
                new TileConfig
                {
                    typeId = new TableDataLink<TileTypeConfig>() { Id = "grass" },
                    position = new Vec2Int(12, 12)
                },
                new TileConfig
                {
                    typeId = new TableDataLink<TileTypeConfig>() { Id = "grass" },
                    position = new Vec2Int(13, 12)
                }
            };
            return item;
        }
    }
}
