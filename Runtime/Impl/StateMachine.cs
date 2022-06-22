using System;
using Packages.HFSM.Runtime.Interfaces;
using UnityEngine;

namespace Packages.HFSM.Runtime.Impl
{
    public class StateMachine : IStateMachine
    {
        private bool _isRunning;

        private IPseudoStateInternal _currentState;
        private readonly string _name;
        private readonly IStateMachineTemplateInternal _template;

        private StateMachine()
        {
        }

        public StateMachine(string stateMachineName)
        {
            _name = stateMachineName;
            _template = new StateMachineTemplate(null, stateMachineName);
        }

        public void Start()
        {
            if (_isRunning)
            {
                throw new Exception("StateMachine is already running.");
            }

            _isRunning = true;
            _currentState = (IPseudoStateInternal)_template.Initial;
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

                if (subState.IsCompositeState())
                {
                    Execute(subState.CurrentState);
                }
                else if (pseudoState is IDoInternal doSubState && doSubState.HasActivity())
                {
                    doSubState.Do();
                }
            }
            else
            {
                ContinueExecution();
            }

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

        public IState CreateState(string stateName = "State")
        {
            return _template.CreateState(stateName);
        }

        public IFinal CreateFinal(string stateName = "Exit")
        {
            return _template.CreateFinal(stateName);
        }
    }
}