using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Packages.HFSM.Runtime.Impl.Data;
using Packages.HFSM.Runtime.Impl.Utils;
using Packages.HFSM.Runtime.Interfaces;

namespace Packages.HFSM.Runtime.Impl
{
    internal class StateMachineTemplate : IStateMachineTemplateInternal
    {
        public IInitial Initial => _initial;
        public IFinal Final => _final;

        private readonly string _templateName;
        private readonly IInitialInternal _initial;
        private IFinalInternal _final;
        private readonly HashSet<IPseudoStateInternal> _states;
        private readonly IStateInternal _parentState;
        private readonly ILogger _logger;
        private readonly IStateMachine _stateMachine;

        internal StateMachineTemplate(IStateMachine stateMachine, IStateInternal parentState, string templateName, [NotNull]ILogger logger)
        {
            _logger = logger;
            _parentState = parentState;
            _templateName = templateName;
            _stateMachine = stateMachine;

            _states = new HashSet<IPseudoStateInternal>(3);
            _initial = StateFactory.CreateInitial($"{_templateName}_Initial", _logger);
        }

        public IState CreateState(string stateName = "State")
        {
            var newState = StateFactory.CreateState(_stateMachine, _parentState, $"{_templateName}_{stateName}", _logger);
            if (_states.Count == 0) ;
            {
                _initial.TransitionTo(newState);
            }
            _states.Add(newState);
            _logger?.Log(LogLevel.Verbose, $"State '{stateName}' created. Count: {_states.Count}");

            return newState;
        }

        public IFinal CreateFinal(string stateName = "Exit")
        {
            if (_final != null)
            {
                throw new Exception("StateMachine already has a final state");
            }
            _final = StateFactory.CreateFinal($"{_templateName}_{stateName}", _logger);
            _logger?.Log(LogLevel.Verbose, $"Final State '{stateName}' created.");

            _final.OnEnter(MarkParentStateAsDone);
            return _final;
            
            void MarkParentStateAsDone()
            {
                _logger?.Log(LogLevel.Verbose, $"Final State '{stateName}' reached. Marking '{_parentState?.Name}' as done.");
                _parentState?.MarkStateAsDone();
            }
        }
    }
}