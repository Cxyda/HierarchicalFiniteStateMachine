namespace HFSM.Interfaces
{
    public interface  IStateMachineEventInternal : IStateMachineEvent
    {
        void Invoke();
        void Reset();
        bool HasBeenInvoked { get; }
    }

    public interface IStateMachineEvent
    {
        string Name { get; }
    }
}