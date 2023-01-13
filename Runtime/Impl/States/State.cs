using System;
using System.Collections.Generic;
using Packages.HFSM.Runtime.Impl.Utils;
using Packages.HFSM.Runtime.Interfaces;

namespace Packages.HFSM.Runtime.Impl.States
{
    public class State : PseudoState, IStateInternal
    {
        public event Action OnStateCompleteEvent;
        public IPseudoStateInternal CurrentState { get; private set; }
        public IStateMachine StateMachine { get; private set; }

        private IStateMachineTemplateInternal _nestedState;
        private readonly IStateInternal _parentState;

        internal State(IStateMachine stateMachine, IStateInternal parentState, string name, int id, ILogger logger) : base(new HashSet<EnterDelegate>(3), new HashSet<ExitDelegate>(3),
            new HashSet<ITransitionInternal>(3), logger)
        {
            StateMachine = stateMachine;
            Id = id;
            Name = name;
            CurrentState = null;
            _parentState = parentState;
        }

        public void OnEnter(EnterDelegate enterDelegate)
        {
            onEnterDelegates.Add(enterDelegate);
        }

        public void OnExit(ExitDelegate exitDelegate)
        {
            onExitDelegates.Add(exitDelegate);
        }

        public void Nest(StateMachineSetupDelegate stateMachineSetup)
        {
            if (_nestedState != null)
            {
                throw new Exception($"State {Name} already has a nested state.");
            }
            if (doActivity != null)
            {
                throw new Exception($"Nested states ('{Name}') may not have do activities.");
            }

            _nestedState = new StateMachineTemplate(StateMachine, this, Name, logger);
            stateMachineSetup(_nestedState);
            if (_nestedState.Final == null)
            {
                throw new Exception($"Nested states ('{Name}') must have a final state.");
            }
            CurrentState = (IPseudoStateInternal)_nestedState.Initial;
        }

        public bool IsDone()
        {
            return onDoActivityDelegate == null || isDone;
        }

        public bool IsCompositeState()
        {
            return _nestedState != null;
        }

        public void Abort()
        {
            doActivity?.Abort();
            Exit();
        }

        public void OnDo(DoActivityDelegate doActivityDelegate)
        {
            if (doActivity != null)
            {
                throw new Exception($"State {Name} already has a Do activity");
            }

            if (_nestedState != null)
            {
                throw new Exception($"Nested states ('{Name}') may not have Do activities.");
            }

            doActivity = new DoActivity(MarkStateAsDone);
            onDoActivityDelegate = doActivityDelegate;
        }

        public override bool Trigger(IStateMachineEvent @event)
        {
            var hasTrigger = base.Trigger(@event);

            if (!hasTrigger)
            {
                if (_parentState == null)
                {
                    return false;
                }

                return _parentState.Trigger(@event);
            }

            MarkStateAsDone();
            return true;
        }

        public void MarkStateAsDone()
        {
            isDone = true;
            OnStateCompleteEvent?.Invoke();
        }

        protected override void OnReset()
        {
            isDone = false;
        }
    }
}