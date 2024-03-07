using Remouse.Simulation;
using Remouse.World;
using ReDI;
using NUnit.Framework;
using Remouse.Utils;

namespace Shared.Test.WorldTests
{
    public class NetworkSimulationTests
    {
        private ContainerBuilder _containerBuilder;

        [SetUp]
        public void SetUp()
        {
            _containerBuilder = new ContainerBuilder(); 
            _containerBuilder.AddModule<SimulationModule>();
            
            LLogger.SetLogger(new TestLLogger());
        }

        [Test]
        public void TestSimulationSendingCorrect()
        {
            var container = _containerBuilder.Build();
            var systemsBuilder = container.Resolve<EcsSystemsBuilder>();
            systemsBuilder.Add<TestSystem>(10);
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

            var filter = simulation.World.Filter<TestComponent>().End();

            int actualEntityCount = 0;
            long targetCount = startCount + passedTicks;
            foreach (var entity in filter)
            {
                var testComponent = simulation.World.GetComponent<TestComponent>(entity);
                Assert.AreEqual(targetCount, testComponent.counter, "Expected counter on component not the same as in the world");
                actualEntityCount++;
            }
            
            Assert.AreEqual(expectedEntityCount, actualEntityCount, "Entity count in world not same as spawned");
        }
    }
}