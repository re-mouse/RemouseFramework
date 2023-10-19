using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Remouse.GameServer.ServerShards;
using Remouse.GameServer.ServerTransport;
using Remouse.Shared.Core;
using Remouse.Shared.Core.World;
using Remouse.Shared.DIContainer;
using Remouse.Shared.Utils.Log;

namespace Remouse.GameServer
{
    public class ServerConfig
    {
        public string databaseJsonPath = "";
        public double ticksPerSecond = 60;
        public string mapId = "Start";
        public ushort port = 5000;
        public int maxPlayers = 10;
    }
    public class ServerBootstrap : IDisposable
    {
        private readonly ServerConfig _config;
        private readonly double _millisecondsPerTick;
        
        private Container _container;
        
        private IServer _server;
        private ServerGameLoop _gameLoop;
        
        private Stopwatch _stopwatch;
        private double _lastTickMilliseconds;

        public ServerBootstrap(ServerConfig config)
        {
            _config = config;
            _millisecondsPerTick = 1000 / _config.ticksPerSecond;
        }

        public void Start()
        {
            var containerBuilder = new ContainerBuilder();
            
            containerBuilder.InstallGameServer();
            containerBuilder.InstallServerTransport();
            containerBuilder.InstallWorld();
            containerBuilder.InstallDatabaseFromJson(_config.databaseJsonPath);

            _container = containerBuilder.Build();

            _server = _container.Get<IServer>();
            _gameLoop = _container.Get<ServerGameLoop>();

            LoadSimulation();
            
            _server.Start(_config.port, _config.maxPlayers);
            
            _stopwatch = Stopwatch.StartNew();
        }

        private void LoadSimulation()
        {
            var simulation = _container.Get<SimulationFactory>().CreateGameSimulation();
            
            _container.Get<SimulationHost>().SetGameSimulation(simulation);
        }

        public void Update()
        {
            _server.Update();
                
            double elapsed = _stopwatch.Elapsed.TotalMilliseconds - _lastTickMilliseconds;
            double requiredTicks = Math.Floor(elapsed / _millisecondsPerTick);

            for (int i = 0; i < requiredTicks; i++)
            {
                _gameLoop.Update();
                _lastTickMilliseconds += _millisecondsPerTick;
            }
        }

        public void Dispose()
        {
            _container?.Dispose();
        }
    }
}