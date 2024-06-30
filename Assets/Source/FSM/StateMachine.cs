using System;
using System.Collections.Generic;

namespace Source.FSM
{
    public class StateMachine<T>
    {
        private const int DefaultStateCount = 8;

        private readonly Dictionary<Type, IState<T>> _states = new(DefaultStateCount);
        private readonly List<Transition<T>> _anyTransitions = new(DefaultStateCount);
        private readonly List<Transition<T>> _transitions = new(DefaultStateCount);

        public StateMachine(params IState<T>[] states)
        {
            AddStates(states);
        }

        public bool TransitionsEnabled { get; set; } = true;

        public bool HasCurrentState { get; private set; }

        public bool HasStatesBeenAdded { get; private set; }

        public IState<T> CurrentState { get; private set; }

        public Transition<T> CurrentTransition { get; private set; }

        public void AddStates(params IState<T>[] states)
        {
            if (HasStatesBeenAdded)
            {
                throw new Exception("States have already been added!");
            }

            if (states.Length == 0)
            {
                throw new Exception("You're trying to add an empty state array!");
            }

            foreach (IState<T> state in states)
            {
                AddState(state);
            }

            HasStatesBeenAdded = true;
        }

        public TState GetState<TState>() where TState : IState<T>
        {
            return (TState)GetState(typeof(TState));
        }

        public void SetState<TState>() where TState : IState<T>
        {
            if (CurrentState is TState)
            {
                return;
            }

            SetState(typeof(TState));
        }

        public void AddTransition<TStateFrom, TStateTo>(Func<bool> condition)
            where TStateFrom : IState<T> where TStateTo : IState<T>
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            IState<T> stateFrom = GetState(typeof(TStateFrom));
            IState<T> stateTo = GetState(typeof(TStateTo));

            _transitions.Add(new Transition<T>(stateFrom, stateTo, condition));
        }

        public void AddAnyTransition<TStateTo>(Func<bool> condition)
            where TStateTo : IState<T>
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            IState<T> stateTo = GetState(typeof(TStateTo));

            _anyTransitions.Add(new Transition<T>(null, stateTo, condition));
        }

        public void SetStateByTransitions()
        {
            CurrentTransition = GetTransition();

            if (CurrentTransition == null)
            {
                return;
            }

            if (CurrentState == CurrentTransition.To)
            {
                return;
            }

            SetState(CurrentTransition.To);
        }

        public void Run()
        {
            if (TransitionsEnabled)
            {
                SetStateByTransitions();
            }

            if (HasCurrentState)
            {
                CurrentState.Update();
            }
        }

        private void AddState(IState<T> state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            Type stateType = state.GetType();

            if (_states.TryAdd(stateType, state) == false)
            {
                throw new Exception($"You're trying to add the same state twice! The <{stateType}> already exists!");
            }
        }

        private IState<T> GetState(Type type)
        {
            if (_states.TryGetValue(type, out IState<T> state))
            {
                return state;
            }

            throw new Exception($"You didn't add the <{type}> state!");
        }

        private void SetState(Type type)
        {
            IState<T> state = GetState(type);

            SetState(state);
        }

        private void SetState(IState<T> state)
        {
            if (HasCurrentState)
            {
                CurrentState.Exit();
            }

            CurrentState = state;
            HasCurrentState = true;
            CurrentState.Enter();
        }

        private Transition<T> GetTransition()
        {
            for (int i = 0; i < _anyTransitions.Count; i++)
            {
                if (_anyTransitions[i].Condition.Invoke())
                {
                    return _anyTransitions[i];
                }
            }

            for (int i = 0; i < _transitions.Count; i++)
            {
                if (_transitions[i].From != CurrentState)
                {
                    continue;
                }

                if (_transitions[i].Condition.Invoke())
                {
                    return _transitions[i];
                }
            }

            return null;
        }
    }
}