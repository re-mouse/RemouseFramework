using System;
using System.Diagnostics;
using Remouse.GameServer.ServerTransport;
using Remouse.Core;
using Remouse.DIContainer;

namespace Remouse.GameServer
{
    [Serializable]
    public class ServerConfig
    {
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
        
        private IPlayerServer _playerServer;
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
            containerBuilder.AddModule<GameServerCoreModule>();

            _container = containerBuilder.Build();

            _playerServer = _container.Resolve<IPlayerServer>();
            _gameLoop = _container.Resolve<ServerGameLoop>();

            LoadSimulation();
            
            _playerServer.Start(_config.port, 40);
            
            _stopwatch = Stopwatch.StartNew();
        }

        private void LoadSimulation()
        {
            var simulation = _container.Resolve<SimulationFactory>().CreateGameSimulation(_config.mapId);
            
            _container.Resolve<SimulationHost>().Set(simulation);
        }

        public void Update()
        {
            _playerServer.Update();
                
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