using System.Collections.Generic;

namespace CoxlinFSM
{
    public class StateMachine
    {
        private readonly Dictionary<State, Transition[]> _transitionMap = new Dictionary<State, Transition[]>();
        private State _currentState, _newState;
        private Transition[] _currentTransitions;
        private int _transitionCount = -1;
        private bool _isRunning = false;

        public void Start() => _isRunning = true;
        public void Stop() => _isRunning = false;

        public void AddState(
            State state,
            Transition[] transitions)
        {
            if (!_transitionMap.ContainsKey(state))
            {
                _transitionMap[state] = transitions;
            }
        }

        public void RemoveState(State state)
        {
            if (_transitionMap.ContainsKey(state))
            {
                _transitionMap.Remove(state);
            }
        }

        public void AddTransition(State state, Transition transition)
        {
            if (!_transitionMap.ContainsKey(state))
            {
                return;
            }
            var list = new List<Transition>(_transitionMap[state]);
            list.Add(transition);
            _transitionMap[state] = list.ToArray();
        }

        public void RemoveTransition(State state, Transition transition)
        {
            if (!_transitionMap.ContainsKey(state))
            {
                return;
            }
            var list = new List<Transition>(_transitionMap[state]);
            list.Remove(transition);
            _transitionMap[state] = list.ToArray();
        }

        public void SetState(State nextState) => _newState = nextState;

        public void Update(float deltaTime)
        {
            if (!_isRunning)
            {
                return;
            }

            if (_currentState != _newState)
            {
                LeaveCurrentStateAndEnterNewState();
            }

            if (_currentTransitions.Length > 0)
            {
                if (_transitionCount != _currentTransitions.Length)
                {
                    _transitionCount = _currentTransitions.Length;
                }

                for (int i = 0; i < _transitionCount; ++i)
                {
                    if (_currentTransitions[i].ShouldTransitionToState())
                    {
                        SetState(_currentTransitions[i].StateToTransitionTo);
                        return;
                    }
                }
            }

            _currentState.OnUpdate(deltaTime);
        }

        private void LeaveCurrentStateAndEnterNewState()
        {
            if (_currentState != null)
            {
                _currentState.OnExit();
            }
            _newState.OnEnter();

            _currentState = _newState;

            _currentTransitions = new Transition[0];
            if (_transitionMap.ContainsKey(_currentState))
            {
                _currentTransitions = _transitionMap[_currentState];
            }
        }

        public void TearDown()
        {
            _transitionMap.Clear();
            _currentState = _newState = null;
            _currentTransitions = null;
            _transitionCount = -1;
            _isRunning = false;
        }
    }
}
