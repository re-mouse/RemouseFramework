using NUnit.Framework;
using Remouse.Core.Configs;
using System.Collections.Generic;
using Remouse.Models.Database;
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

            var json = new DatabaseSerializer().SerializeToJsonBytes(database);
            database = new DatabaseSerializer().DeserializeFromBytes(json);

            var defaultMap = database.GetTableData<MapConfig>("Map");
            Assert.IsNotNull(defaultMap);
            Assert.AreEqual("Map", defaultMap.id);

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

            var json = new DatabaseSerializer().SerializeToJson(database);
            TestContext.Out.WriteLine(json);
        }

        private static MapConfig GetMapConfig()
        {
            var item = new MapConfig();
            item.id = "Map";
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
                            componentValues = new Dictionary<string, string>
                            {
                                { "speed", 15.ToString() }
                            }
                        },
                        new ComponentConfig
                        {
                            name = "health",
                            componentValues = new Dictionary<string, string>
                            {
                                { "startHealth", 20.ToString() },
                                { "maxHealth", 50.ToString() }
                            }
                        }
                    },
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
            item.id = "Start";
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
                            componentValues = new Dictionary<string, string>
                            {
                                { "speed", 15.ToString() }
                            }
                        },
                        new ComponentConfig
                        {
                            name = "health",
                            componentValues = new Dictionary<string, string>
                            {
                                { "startHealth", 20.ToString() },
                                { "maxHealth", 50.ToString() }
                            }
                        }
                    },
                },
                new EntityConfig()
                {
                    typeId = new TableDataLink<EntityTypeConfig>() { Id = "playerSpawnPoint" },
                    overrideComponents = new ComponentConfig[]
                    {
                        new ComponentConfig
                        {
                            name = "playerCapacity",
                            componentValues = new Dictionary<string, string>
                            {
                                { "capacity", 20.ToString() }
                            }
                        },
                        new ComponentConfig
                        {
                            name = "health",
                            componentValues = new Dictionary<string, string>
                            {
                                { "startHealth", 40.ToString() },
                                { "maxHealth", 50.ToString() }
                            }
                        }
                    },
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
