using LogLevel = Packages.HFSM.Runtime.Impl.Data.LogLevel;

namespace Packages.HFSM.Runtime.Interfaces
{
    public interface IStateMachine : IStateMachineTemplate
    {
        LogLevel LogLevel { get; set; }
        void Start();
        void Stop();
        
        void Trigger(IStateMachineEvent @event);
    }
}
