namespace HFSM.Interfaces
{
    public interface IStateMachine
    {
        void Start();
        void Stop();
        
        void Trigger(IStateMachineEvent @event);

        IInitial CreateInitial(string stateName = "");
        IState CreateState(string stateName = "");
        IFinal CreateFinal(string stateName = "");
    }
}
