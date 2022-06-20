namespace Packages.HFSM.Runtime.Interfaces
{
    public interface IStateMachine : IStateMachineTemplate
    {
        void Start();
        void Stop();
        
        void Trigger(IStateMachineEvent @event);
    }
}
