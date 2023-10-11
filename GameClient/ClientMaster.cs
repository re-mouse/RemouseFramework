using System;
using System.Diagnostics;
using System.Net;
using Shared.DIContainer;
using Shared.GameSimulation;
using Shared.Online.Commands;
using Shared.Online.Commands.Client;

namespace GameClient
{
    public class ClientMaster
    {
        private ClientSocketManager _socketManager;
        private GameSimulationRunner _gameSimulationRunner;
        private GameSimulationUpdatesBuffer _gameSimulationUpdatesBuffer;
        private GameSimulationFactory _simulationFactory;
        
        private GameSimulation _simulation;

        private Stopwatch _sw = Stopwatch.StartNew();
        private double _lastTickMilliseconds = 0;

        private bool _isFirstUpdateReceived;
        
        public void Construct(Container container)
        {
            container.Get(out _socketManager);
            container.Get(out _gameSimulationRunner);
            container.Get(out _gameSimulationUpdatesBuffer);
            container.Get(out _simulationFactory);

            _socketManager.disconnected += HandleDisconnected;
            _socketManager.onMessage += HandleMessage;
        }

        private void HandleMessage(NetworkMessage message)
        {
            Logger.Current.LogInfo(this, $"Received message {message}");
            
            if (message is StartLoadSimulationMessage simulationLoadMessage)
            {
                _simulation = _simulationFactory.CreateGameSimulation(simulationLoadMessage.simulationName);
                
                _socketManager.Send(new SpawnInWorldRequestClientMessage());

                _isFirstUpdateReceived = false;
            } 
            else if (message is SimulationUpdateMessage entityUpdateMessage)
            {
                if (!_isFirstUpdateReceived)
                {
                    _simulation.SetTick(entityUpdateMessage.simulationTick);

                    _gameSimulationRunner.SetSimulation(_simulation);
                    _isFirstUpdateReceived = true;
                }
                
                _gameSimulationUpdatesBuffer.Enqueue(entityUpdateMessage);
            }
        }
        
        public void Update()
        {
            _socketManager.Tick();

            if (_lastTickMilliseconds == 0)
                _lastTickMilliseconds = _sw.Elapsed.TotalMilliseconds;
            
            double elapsed = _sw.Elapsed.TotalMilliseconds - _lastTickMilliseconds;
            double requiredTicks = Math.Floor(elapsed / Const.MillisecondsPerTick);

            for (int i = 0; i < requiredTicks; i++)
            {
                _lastTickMilliseconds += Const.MillisecondsPerTick;
                
                _gameSimulationRunner.Run();
            }
        }

        public void Connect(IPEndPoint endPoint)
        {
            _socketManager.Start(endPoint);
        }

        public void Disconnect()
        {
            _socketManager.Stop();
        }
        
        private void HandleDisconnected()
        {
            Logger.Current.LogInfo(this, "Disconnected");
            _simulation = null;
        }
    }
}