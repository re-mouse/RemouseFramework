using System;
using System.Diagnostics;
using GameServer.ServerTransport;
using Shared.DIContainer;
using Shared.Utils.Log;

namespace GameServer
{
    public class ServerBootstrap : IDisposable
    {
        private readonly double _millisecondsPerTick;
        
        private Container _container;
        private bool _isStarted;
        private bool _isDisposed;
        
        private IServer _server;
        private ServerLoop _serverLoop;
        
        private Stopwatch _stopwatch;
        private double _lastTickMilliseconds;

        public ServerBootstrap(double ticksPerSecond)
        {
            _millisecondsPerTick = 1000 / ticksPerSecond;
        }

        public async void Start(double ticksPerSecond)
        {
            if (_isDisposed)
            {
                Logger.Current.LogError(this, "Trying to start disposed server bootstrap");
                return;
            }
            
            if (_isStarted)
            {
                Logger.Current.LogError(this, "Trying to start bootstrap, when already started");
                return;
            }
            
            var containerBuilder = new ContainerBuilder();
            
            var serverLoopScope = new ServerLoopScope();
            var serverScope = new ServerTransportScope();
            
            serverLoopScope.InstallScope(containerBuilder);
            serverScope.InstallScope(containerBuilder);

            _container = containerBuilder.Build();

            _server = _container.Get<IServer>();
            _serverLoop = _container.Get<ServerLoop>();

            _server.Start(5000);
            await _serverLoop.Initialize();
            
            _stopwatch = Stopwatch.StartNew();

            _isStarted = true;
        }
        
        public void Update()
        {
            if (_isDisposed)
            {
                Logger.Current.LogError(this, "Trying to update disposed server bootstrap");
                return;
            }
            
            if (!_isStarted)
            {
                Logger.Current.LogError(this, "Trying to update, without started");
                return;
            }
            
            _server.Update();
                
            double elapsed = _stopwatch.Elapsed.TotalMilliseconds - _lastTickMilliseconds;
            double requiredTicks = Math.Floor(elapsed / _millisecondsPerTick);

            for (int i = 0; i < requiredTicks; i++)
            {
                _serverLoop.Update();
                _lastTickMilliseconds += _millisecondsPerTick;
            }
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                Logger.Current.LogError(this, "Trying to dispose server bootstrap twice");
                return;
            }
            
            _container?.Dispose();
            _isDisposed = true;
        }
    }
}