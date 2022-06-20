using System;
using System.Collections.Generic;
using Packages.HFSM.Runtime.Interfaces;

namespace Packages.HFSM.Runtime.Impl.States
{
    public class State : PseudoState, IStateInternal
    {
        public event Action OnStateCompleteEvent;

        internal State(string name, int id) : base( new HashSet<EnterDelegate>(3),  new HashSet<ExitDelegate>(3), new HashSet<ITransitionInternal>(3))
        {
            Id = id;
            Name = name;
        }

        public void OnEnter(EnterDelegate enterDelegate)
        {
            onEnterDelegates.Add(enterDelegate);
        }

        public void OnExit(ExitDelegate exitDelegate)
        {
            onExitDelegates.Add(exitDelegate);
        }

        public bool IsDone()
        {
            return onDoActivityDelegate == null || isDone;
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