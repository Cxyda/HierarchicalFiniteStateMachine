using System;
using System.Collections.Generic;
using Packages.HFSM.Runtime.Interfaces;

namespace Packages.HFSM.Runtime.Impl.States
{
    public class State : PseudoState, IStateInternal
    {
        public event Action OnStateCompleteEvent;

        public IPseudoStateInternal CurrentState { get; private set; }

        private StateMachineTemplate _nestedState;

        internal State(string name, int id) : base(new HashSet<EnterDelegate>(3), new HashSet<ExitDelegate>(3),
            new HashSet<ITransitionInternal>(3))
        {
            Id = id;
            Name = name;
            CurrentState = null;
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
            _nestedState = new StateMachineTemplate(Name);
            stateMachineSetup(_nestedState);
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

            doActivity = new DoActivity(MarkStateAsDone);
            onDoActivityDelegate = doActivityDelegate;
        }

        public override bool Trigger(IStateMachineEvent @event)
        {
            var hasTrigger = base.Trigger(@event);
            if (!hasTrigger)
            {
                return false;
            }

            MarkStateAsDone();
            return true;
        }

        private void MarkStateAsDone()
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