using System;
using System.Diagnostics.CodeAnalysis;
using Packages.HFSM.Runtime.Impl.Data;
using Packages.HFSM.Runtime.Impl.Utils;
using Packages.HFSM.Runtime.Interfaces;
using ILogger = UnityEngine.ILogger;

namespace Packages.HFSM.Runtime.Impl
{
    public class StateMachine : IStateMachine
    {
        private bool _isRunning;

        private IPseudoStateInternal _currentState;
        private readonly string _name;
        private readonly IStateMachineTemplateInternal _template;


        public LogLevel LogLevel
        {
            get => _logger.logLevel;
            set => _logger.logLevel = value;
        }
        public bool IsRunning => _isRunning;

        [NotNull]private readonly UnityLogger _logger;

        private StateMachine()
        {
        }

        public StateMachine(string stateMachineName, ILogger logger = null)
        {
            _name = stateMachineName;
            if (logger == null)
            {
                _logger = new UnityLogger(LogLevel.Error, _name);
            }

            _template = new StateMachineTemplate(this, null, stateMachineName, _logger ?? throw new InvalidOperationException());
            _logger.Log(LogLevel.Verbose, "Initialized.");
        }

        public void Start()
        {
            if (_isRunning)
            {
                throw new Exception("StateMachine is already running.");
            }
            _logger.Log(LogLevel.Verbose, "Started.");

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
            _logger.Log(LogLevel.Log, $"Stopped.");
        }

        private void Execute(IPseudoStateInternal pseudoState)
        {
            _logger.Log(LogLevel.Log, $"--> Executing '{pseudoState.Name}'...");

            if (pseudoState is IEnterInternal enterSubState)
            {
                _logger.Log(LogLevel.Info, $"--> Entering '{pseudoState.Name}'...");
                enterSubState.Enter();
            }

            IStateInternal subState = pseudoState as IStateInternal;
            if (subState != null)
            {
                subState.OnStateCompleteEvent += ContinueExecution;

                if (subState.IsCompositeState())
                {
                    _logger.Log(LogLevel.Info, $"--> Entering nested state '{subState.CurrentState}' of '{pseudoState.Name}'...");
                    Execute(subState.CurrentState);
                    return;
                }
                if (pseudoState is IDoInternal doSubState && doSubState.HasActivity())
                {
                    _logger.Log(LogLevel.Info, $"--> DoActivity of '{pseudoState.Name}'...");
                    doSubState.Do();
                    return;
                }

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

                    if (subState.IsCompositeState())
                    {
                        ((IStateInternal)_currentState)?.Abort();
                    }
                }
                if (pseudoState is IExitInternal exitSubState)
                {
                    _logger.Log(LogLevel.Info, $"--> Exiting '{pseudoState.Name}'...");
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
                IPseudoStateInternal target = null;
                if (!isTrue)
                {
                    if (transition.AlternativeTarget != null)
                    {
                        target = (IPseudoStateInternal)transition.AlternativeTarget;
                        _logger.Log(LogLevel.Verbose, $"--> {state.Name} --> Else Transition condition to '{target.Name}' evaluated TRUE");
                        return target;
                    }
                    continue;
                }

                target = (IPseudoStateInternal)transition.Target;
                _logger.Log(LogLevel.Verbose, $"--> {state.Name} --> Transition condition to '{target.Name}' evaluated TRUE");
                return target;
            }
            _logger.Log(LogLevel.Verbose, $"--> {state.Name} has no outgoing transitions. Waiting for events....");
            return null;
        }

        public void Trigger(IStateMachineEvent @event)
        {
            var hasTrigger = _currentState.Trigger(@event);

            if (!hasTrigger)
            {
                _logger.Log(LogLevel.Warning, $"--> State {_currentState.Name} does not have a trigger for {@event.Name}");
            }
        }

        public IState CreateState(string stateName = "State")
        {
            return _template.CreateState(stateName);
        }

        public IFinal CreateFinal(string stateName = "Final")
        {
            return _template.CreateFinal(stateName);
        }
    }
}