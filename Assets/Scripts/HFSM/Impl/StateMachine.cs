using System;
using System.Collections.Generic;
using HFSM.Interfaces;
using UnityEngine;

namespace HFSM.Impl
{
    public class StateMachine : IStateMachine
    {
        private bool _isRunning;

        private IInitialInternal _initial;
        private IFinalInternal _final;
        private readonly HashSet<IPseudoStateInternal> _states;

        private IPseudoStateInternal _currentState;
        private readonly string _name;

        internal StateMachine(string stateChartName)
        {
            _name = stateChartName;
            _states = new HashSet<IPseudoStateInternal>(3);
        }

        public void Start()
        {
            if (_isRunning)
            {
                throw new Exception("StateMachine is already running.");
            }

            _isRunning = true;
            _currentState = _initial;
            Execute(_currentState);
        }

        public void Stop()
        {
            _isRunning = false;
            if (_currentState is IStateInternal state)
            {
                if (!state.IsDone())
                {
                    state.Abort();
                }
            }
        }

        private void Execute(IPseudoStateInternal pseudoState)
        {
            if (pseudoState is IEnterInternal enterSubState)
            {
                enterSubState.Enter();
            }

            IStateInternal subState = pseudoState as IStateInternal;
            if (subState != null)
            {
                subState.OnStateCompleteEvent += ContinueExecution;
            }

            if (pseudoState is IDoInternal doSubState && doSubState.HasActivity())
            {
                doSubState.Do();
                return;
            }

            ContinueExecution();

            void ContinueExecution()
            {
                var newTarget = EvaluateTransitions(pseudoState);
                if (newTarget == null)
                {
                    return;
                }
                if (subState != null)
                {
                    subState.OnStateCompleteEvent -= ContinueExecution;

                    if (!subState.IsDone())
                    {
                        throw new Exception($"Something went wrong. State {pseudoState.Name} is not done but transitions are about to be evaluated.");
                    }
                }
                if (pseudoState is IExitInternal exitSubState)
                {
                    exitSubState.Exit();
                }

                // Reset the state
                _currentState.Reset();
                _currentState = newTarget;
                Execute(_currentState);
            }
        }

        private IPseudoStateInternal EvaluateTransitions(IPseudoStateInternal state)
        {
            foreach (var transition in state.Transitions)
            {
                var isTrue = transition.Evaluate();
                if (!isTrue)
                {
                    if (transition.AlternativeTarget != null)
                    {
                        return (IPseudoStateInternal)transition.AlternativeTarget;
                    }
                    continue;
                }

                return (IPseudoStateInternal)transition.Target;
            }

            return null;
        }

        public void Trigger(IStateMachineEvent @event)
        {
            var hasTrigger = _currentState.Trigger(@event);

            if (!hasTrigger)
            {
                Debug.LogWarning($"State {_currentState.Name} does not have a trigger for {@event.Name}");
            }
        }

        public IInitial CreateInitial(string stateName = "Initial")
        {
            if (_initial != null)
            {
                throw new Exception("StateMachine already has an initial state");
            }

            _initial = StateFactory.CreateInitial($"{_name}_{stateName}");
            return _initial;
        }

        public IState CreateState(string stateName = "State")
        {
            var newState = StateFactory.CreateState($"{_name}_{stateName}");
            _states.Add(newState);

            return newState;
        }

        public IFinal CreateFinal(string stateName = "Exit")
        {
            if (_final != null)
            {
                throw new Exception("StateMachine already has a final state");
            }
            _final = StateFactory.CreateFinal($"{_name}_{stateName}");

            return _final;
        }
    }
}