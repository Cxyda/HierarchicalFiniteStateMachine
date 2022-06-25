namespace Packages.HFSM.Runtime.Interfaces
{
    public interface IStateMachineTemplateInternal : IStateMachineTemplate
    {
        IInitial Initial { get; }
        IFinal Final { get; }
    }

    public interface IStateMachineTemplate
    {
        IState CreateState(string stateName = "State");
        IFinal CreateFinal(string stateName = "Final");
    }
}