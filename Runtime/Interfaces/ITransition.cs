using Packages.HFSM.Runtime.Impl;

namespace Packages.HFSM.Runtime.Interfaces
{
    public interface ITransitionInternal : ITransition
    {
        IStateMachineEventInternal Trigger { get; }
        IEnter Target { get; }
        IEnter AlternativeTarget { get; }
        IPseudoStateInternal Origin { get; set; }
        bool Evaluate();
    }

    public interface ITransition
    {
        ITransition If(ConditionDelegate conditionDelegateDelegate);
        void Else(IEnter target);
        ITransition On(IStateMachineEvent @event);
    }
}