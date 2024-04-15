using System;
using System.Collections.Generic;

namespace Common.States
{
    public class StateMachine: IDisposable
    {
        private Dictionary<Type, State> _states;
        private State _currentState;

        public StateMachine()
        {
            _states = new Dictionary<Type, State>();
        }

        public void AddState(State state)
        {
            Type stateType = state.GetType();

            if (_states.ContainsKey(stateType))
                throw new ArgumentException($"You have already added state of this type {stateType} to state machine");

            _states.Add(stateType, state);

            if (_currentState == null)
            {
                state.Enter();
                _currentState = state;
            }
        }

        public void ChangeState<TState>() where TState: State
        {
            if (!_states.ContainsKey(typeof(TState)))
                throw new Exception($"You are trying to acccess to not existing state of state machine, type of {typeof(TState)}");

            _currentState?.Exit();
            _currentState = _states[typeof(TState)];
            _currentState?.Enter();
        }

        public void Dispose()
        {
            _currentState?.Exit();
        }

        public void Update()
        {
            _currentState?.Update();
        }
    }

}