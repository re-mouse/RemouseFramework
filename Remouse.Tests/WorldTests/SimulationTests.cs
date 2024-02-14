using Remouse.Simulation;
using Remouse.World;
using Remouse.DI;
using NUnit.Framework;
using Remouse.Network.Sockets;
using Remouse.Utils;

namespace Shared.Test.WorldTests
{
    public class SimulationTests
    {
        private ContainerBuilder _containerBuilder;
        private Container _container;

        [SetUp]
        public void SetUp()
        {
            _containerBuilder = new ContainerBuilder(); 
            _containerBuilder.AddModule<SimulationModule>();
            _container = _containerBuilder.Build();
            
            LLogger.SetLogger(new TestLLogger());
        }

        [Test]
        public void TestSimulationBuilding()
        {
            var container = _containerBuilder.Build();
            var worldBuilder = container.Resolve<IWorldBuilder>();
            worldBuilder.AddSystem<TestSystem>();
            
            var simulationLoader = container.Resolve<ISimulationLoader>();

            simulationLoader.LoadEmpty();
            
            var simulation = container.Resolve<ISimulationHost>().Simulation;
            var commandRunner = container.Resolve<ICommandRunner>();
            
            long startCount = 10;
            long passedTicks = 0;
            int expectedEntityCount = 1;
            for (int i = 0; i < expectedEntityCount; i++)
                commandRunner.EnqueueCommand(new TestSpawnCommand() {startingSpawnCounter = 10});

            commandRunner.RunEnqueuedCommands();
            
            simulation.Tick();
            passedTicks++;
            
            simulation.Tick();
            passedTicks++;

            simulation.Tick();
            passedTicks++;

            var filter = simulation.World.Query().Inc<TestComponent>().End();

            int actualEntityCount = 0;
            long targetCount = startCount + passedTicks;
            foreach (var entity in filter)
            {
                var testComponent = simulation.World.GetComponent<TestComponent>(entity);
                Assert.AreEqual(testComponent.counter, targetCount, "Expected counter on component not the same as in the world");
                actualEntityCount++;
            }
            
            Assert.AreEqual(expectedEntityCount, actualEntityCount, "Entity count in world not same as spawned");
        }
    }
}