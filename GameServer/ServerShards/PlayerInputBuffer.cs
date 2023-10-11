using System;
using System.Collections.Generic;
using GameServer.ServerShards;
using Shared.DIContainer;
using Shared.Online.Commands.Client;
using Shared.Utils.Buffer;
using Shared.Utils.Log;

namespace GameServer
{
    public class PlayerInputBuffer
    {
        private const int InputBufferCapacity = 30;
        private const int InputExpireTicksTime = 70;

        private RoundBuffer<HashSet<InputNetworkMessage>> _roundBuffer = new RoundBuffer<HashSet<InputNetworkMessage>>(InputBufferCapacity);;
        
        private GameSimulationHolder _simulationHolder;
        private long _bufferCurrentTick;
        
        private long CurrentTick { get { return _simulationHolder?.simulation?.simulationTick ?? 0; } }

        private void Construct(Container container)
        {
            container.Get(out _simulationHolder);
            
            _bufferCurrentTick = _simulationHolder.simulation.simulationTick;
        }

        //input accepting only on next tick
        public void Enqueue(InputNetworkMessage inputMessage)
        {
            //get inputs
            //apply them
            //run simulation
            //increase tick count
            //look for next inputs
            
            
            //450   ,   450
            UpdateBuffer();
            
            long tickDifference = (int)(inputMessage.simulationTick - _bufferCurrentTick);
            
            if (tickDifference < -InputExpireTicksTime)
            {
                Logger.Current.LogException(this, new ArgumentOutOfRangeException($"Tick lower than current. Received {tick}, current - {CurrentTick}"));
                return;
            }

            if (tickDifference > InputBufferCapacity)
            {
                Logger.Current.LogException(this, new ArgumentOutOfRangeException($"Tick more in future than capacity. Received {tick}, current - {CurrentTick}"));
                return;
            }

            int bufferIndex = tickDifference < 1 ? 1 : (int)tickDifference;
            
            var inputs = _roundBuffer.GetAt(bufferIndex);
            inputMessage.simulationTick = _bufferCurrentTick;
            inputs.Add(inputMessage);
        }
        
        public HashSet<InputNetworkMessage> GetInputMessages()
        {
            return _roundBuffer.GetCurrent();
        }

        private void UpdateBuffer()
        {
            long inputsToClear = CurrentTick - _bufferCurrentTick;

            if (inputsToClear == 0) return;

            if (inputsToClear < 0)
            {
                Logger.Current.LogException(this, new Exception($"Undefined behaviour, simulation went back in time. Difference in ticks {inputsToClear}"));
                return;
            }

            if (inputsToClear > InputBufferCapacity)
                inputsToClear = InputBufferCapacity;

            _bufferCurrentTick = CurrentTick;
            for (int i = 0; i < inputsToClear; i++)
            {
                _roundBuffer.GetCurrent().Clear();
                _roundBuffer.MoveToNext();
            }
        }
    }
}