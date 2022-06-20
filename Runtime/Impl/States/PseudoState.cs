using System.Collections.Generic;
using Packages.HFSM.Runtime.Interfaces;

namespace Packages.HFSM.Runtime.Impl.States
{
    public abstract class PseudoState : IPseudoStateInternal
    {
        public int Id { get; protected set; }
        public string Name { get; protected set; }

        protected bool isDone;

        protected readonly HashSet<EnterDelegate> onEnterDelegates;
        protected readonly HashSet<ExitDelegate> onExitDelegates;
        protected DoActivityDelegate onDoActivityDelegate;

        protected IDoActivity doActivity;
        protected readonly HashSet<ITransitionInternal> transitions;

        public IEnumerable<ITransitionInternal> Transitions => transitions;

        protected PseudoState(HashSet<EnterDelegate> onEnterDelegates, HashSet<ExitDelegate> onExitDelegates, HashSet<ITransitionInternal> transitions)
        {
            this.onEnterDelegates = onEnterDelegates;
            this.onExitDelegates = onExitDelegates;
            this.transitions = transitions;
            doActivity = null;
        }

        public void Enter()
        {
            foreach (var onEnter in onEnterDelegates)
            {
                onEnter.Invoke();
            }
        }

        public void Do()
        {
            onDoActivityDelegate.Invoke(doActivity);
        }

        public void Exit()
        {
            foreach (var onExit in onExitDelegates)
            {
                onExit.Invoke();
            }
        }

        public ITransition TransitionTo(IEnter targetState)
        {
            var transition = new Transition(this, targetState);
            transitions.Add(transition);

            return transition;
        }
        
        public bool HasActivity()
        {
            return doActivity != null;
        }

        public virtual bool Trigger(IStateMachineEvent @event)
        {
            foreach (var transition in transitions)
            {
                if (transition.Trigger.Equals(@event))
                {
                    transition.Trigger.Invoke();
                    return true;
                }
            }

            return false;
        }

        public void Reset()
        {
            foreach (var transition in transitions)
            {
                transition?.Trigger?.Reset();
            }
            OnReset();
        }

        protected virtual void OnReset()
        {
        }
    }
}