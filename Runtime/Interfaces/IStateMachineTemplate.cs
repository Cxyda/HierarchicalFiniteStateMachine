namespace Packages.HFSM.Runtime.Interfaces
{
    public interface IStateMachineTemplateInternal : IStateMachineTemplate
    {
        IInitial Initial { get; }
    }

    public interface IStateMachineTemplate
    {
        IState CreateState(string stateName = "");
        IFinal CreateFinal(string stateName = "");
    }
}