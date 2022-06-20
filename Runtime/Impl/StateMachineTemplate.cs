using System;
using System.Collections.Generic;
using Packages.HFSM.Runtime.Interfaces;

namespace Packages.HFSM.Runtime.Impl
{
    internal class StateMachineTemplate : IStateMachineTemplateInternal
    {
        public IInitial Initial => _initial;

        private readonly string _templateName;
        private readonly IInitialInternal _initial;
        private IFinalInternal _final;
        private readonly HashSet<IPseudoStateInternal> _states;

        internal StateMachineTemplate(string templateName)
        {
            _templateName = templateName;
            _states = new HashSet<IPseudoStateInternal>(3);
            _initial = StateFactory.CreateInitial($"{_templateName}_Initial");
        }

        public IState CreateState(string stateName = "State")
        {
            var newState = StateFactory.CreateState($"{_templateName}_{stateName}");
            if (_states.Count == 0) ;
            {
                _initial.TransitionTo(newState);
            }
            _states.Add(newState);

            return newState;
        }

        public IFinal CreateFinal(string stateName = "Exit")
        {
            if (_final != null)
            {
                throw new Exception("StateMachine already has a final state");
            }
            _final = StateFactory.CreateFinal($"{_templateName}_{stateName}");

            return _final;
        }
    }
}