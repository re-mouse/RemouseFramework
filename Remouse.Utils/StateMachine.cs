using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Remouse.Utils;

namespace Remouse.Utils
{
    public abstract class BaseState
    {
        public abstract UniTask OnStateEnter();
        public abstract UniTask OnStateExit();

        public virtual void Update()
        {
            
        }
    }
    
    public class StateMachine
    {
        private Dictionary<Type, BaseState> _registeredStates = new Dictionary<Type, BaseState>();
        private BaseState _currentState;
        private BaseState _nextState;
        private bool _isTransitioning;

        public void RegisterState<TState>(TState state) where TState : BaseState
        {
            _registeredStates[typeof(TState)] = state;
        }

        public async void SetState<TState>() where TState : BaseState
        {
            var type = typeof(TState);
            if (!_registeredStates.ContainsKey(type))
            {
                LLogger.Current.LogError(this, $"Trying to set not registered state: {type.Name}");
                return;
            }
            
            _nextState = _registeredStates[type];
            LLogger.Current.LogTrace(this, $"Requested state [CurrentState:{_currentState}] [NextState:{_nextState}]");
            
            if (_isTransitioning)
            {
                return;
            }

            while (_nextState != null)
            {
                LLogger.Current.LogTrace(this, $"Start transitioning [CurrentState:{_currentState}] [NextState:{_nextState}]");

                _isTransitioning = true;
                if (_currentState != null)
                {
                    LLogger.Current.LogTrace(this, $"Start exiting from state [ExitingState:{_currentState}]");
                    await _currentState.OnStateExit();
                }

                _currentState = _nextState;
                _nextState = null;

                LLogger.Current.LogTrace(this, $"Start entering in state [EnteringState:{_currentState}]");
                await _currentState.OnStateEnter();

                _isTransitioning = false;
            }
        }

        public void Update()
        {
            if (_currentState != null && !_isTransitioning)
            {
                _currentState.Update();
            }
        }
    }
}