using System;
using System.Diagnostics;
using Remouse.GameServer.ServerTransport;
using Remouse.Core;
using Remouse.DatabaseLib;
using Remouse.DIContainer;
using Remouse.Infrastructure;

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
    
    public class ServerBootstrap
    {
        private ServerConfig _config;
        private double _millisecondsPerTick;
        
        private IPlayerServer _playerServer;
        private ServerGameLoop _gameLoop;
        
        private Stopwatch _stopwatch = new Stopwatch();
        private double _lastTickMilliseconds;
        private SimulationHost _simulationHost;
        private MapLoader _mapLoader;
        private IResources _resources;
        private DatabaseHost _databaseHost;

        public void Construct(Container container)
        {
            _playerServer = container.Resolve<IPlayerServer>();
            _gameLoop = container.Resolve<ServerGameLoop>();
            _mapLoader = container.Resolve<MapLoader>();
            _databaseHost = container.Resolve<DatabaseHost>();
            _resources = container.Resolve<IResources>();
            _simulationHost = container.Resolve<SimulationHost>();
        }

        public void Start(ServerConfig config)
        {
            _config = config;
            _millisecondsPerTick = 1000 / _config.ticksPerSecond;

            _databaseHost.LoadFromResources();

            LoadSimulation();
            
            _playerServer.Start(_config.port, 40);
            
            _stopwatch = Stopwatch.StartNew();
        }

        private void LoadSimulation()
        {
            var simulation = _mapLoader.Load(_config.mapId);
            
            _simulationHost.Set(simulation);
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
    }
}