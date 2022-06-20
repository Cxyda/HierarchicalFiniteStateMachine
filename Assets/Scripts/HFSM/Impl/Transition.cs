using System;
using HFSM.Interfaces;

namespace HFSM.Impl
{
    public class Transition : ITransitionInternal
    {
        public IStateMachineEventInternal Trigger => _event;
        public IEnter Target { get; }
        public IEnter AlternativeTarget { get; private set; }

        public IPseudoStateInternal Origin { get; set; }

        private ConditionDelegate _conditionDelegate;
        private IStateMachineEventInternal _event;

        internal Transition(IPseudoStateInternal origin, IEnter target)
        {
            Origin = origin;
            Target = target;
        }

        public bool Evaluate()
        {
            return (_event == null || _event.HasBeenInvoked) && (_conditionDelegate == null || _conditionDelegate.Invoke());
        }

        public ITransition If(ConditionDelegate conditionDelegateDelegate)
        {
            ValidateTransition();
            if (_conditionDelegate != null)
            {
                throw new Exception($"Transition to {((IPseudoState)Target).Name} already has a condition.");
            }
            _conditionDelegate = conditionDelegateDelegate;
            return this;
        }

        public void Else(IEnter target)
        {
            AlternativeTarget = target;
        }

        public ITransition When(IStateMachineEvent @event)
        {
            ValidateTransition();

            if (_event != null)
            {
                throw new Exception($"Transition to {((IPseudoState)Target).Name} already has a trigger event.");
            }

            _event = (IStateMachineEventInternal)@event;
            return this;
        }
        
        private void ValidateTransition()
        {
            if(Origin is IInitial)
            {
                throw new Exception(
                    $"{nameof(IInitial)} state transitions are not allowed to have Triggers or Conditions");
            }
        }
    }
}