using NUnit.Framework;
using Remouse.Core.Configs;
using System.Collections.Generic;
using System.Linq;
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

            var json = new DatabaseJsonSerializer().SerializeToJsonBytes(database);
            database = new DatabaseJsonSerializer().DeserializeFromBytes(json);

            var defaultMap = database.GetTableData<MapConfig>("Map");
            Assert.IsNotNull(defaultMap);
            Assert.AreEqual("Map", defaultMap.id);

            // Проверка для EntityConfig
            var defaultEntity = defaultMap.entities[0];
            Assert.AreEqual("monster", defaultEntity.typeId.Id);
    
            // Проверка для ComponentConfig
            var movementSpeedComponent = defaultEntity.overrideComponents[0];
            Assert.AreEqual("movementSpeed", movementSpeedComponent.componentType);
            Assert.AreEqual("15", movementSpeedComponent.fieldValues.FirstOrDefault(f => f.fieldName == "speed").value);
    
            var healthComponent = defaultEntity.overrideComponents[1];
            Assert.AreEqual("health", healthComponent.componentType);
            Assert.AreEqual("20", healthComponent.fieldValues.FirstOrDefault(f => f.fieldName == "startHealth").value);
            Assert.AreEqual("50", healthComponent.fieldValues.FirstOrDefault(f => f.fieldName == "maxHealth").value);
    
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

            var json = new DatabaseJsonSerializer().SerializeToJson(database);
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
                    overrideComponents = new List<ComponentConfig>()
                    {
                        new ComponentConfig
                        {
                            componentType = "movementSpeed",
                            fieldValues = new List<ComponentFieldValue>()
                            {
                                new ComponentFieldValue("speed", 15.ToString())
                            }
                        },
                        new ComponentConfig
                        {
                            componentType = "health",
                            fieldValues = new List<ComponentFieldValue>()
                            {
                                new ComponentFieldValue( "startHealth", 20.ToString() ),
                                new ComponentFieldValue( "maxHealth", 50.ToString() )
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
                    overrideComponents = new List<ComponentConfig>()
                    {
                        new ComponentConfig
                        {
                            componentType = "movementSpeed",
                            fieldValues = new List<ComponentFieldValue>()
                            {
                                new ComponentFieldValue( "speed", 15.ToString())
                            }
                    },
                        new ComponentConfig
                        {
                            componentType = "health",
                            fieldValues = new List<ComponentFieldValue>()
                            {
                                new ComponentFieldValue( "startHealth", 20.ToString() ),
                                new ComponentFieldValue( "maxHealth", 50.ToString() )
                            }
                        }
                    },
                },
                new EntityConfig()
                {
                    typeId = new TableDataLink<EntityTypeConfig>() { Id = "playerSpawnPoint" },
                    overrideComponents = new List<ComponentConfig>()
                    {
                        new ComponentConfig
                        {
                            componentType = "playerCapacity",
                            fieldValues = new List<ComponentFieldValue>()
                            {
                                new ComponentFieldValue( "capacity", 20.ToString() )
                        }
                    },
                        new ComponentConfig
                        {
                            componentType = "health",
                            fieldValues = new List<ComponentFieldValue>()
                            {
                                new ComponentFieldValue( "startHealth", 40.ToString() ), 
                                new ComponentFieldValue( "maxHealth", 50.ToString() )
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
