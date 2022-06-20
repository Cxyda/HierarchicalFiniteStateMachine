using System.Collections.Generic;

namespace HFSM.Interfaces
{
    public interface IPseudoStateInternal : IPseudoState
    {
        IEnumerable<ITransitionInternal> Transitions { get; }
        bool Trigger(IStateMachineEvent @event);
        void Reset();
    }

    public interface IPseudoState
    {
        int Id { get; }
        string Name { get; }
    }
}