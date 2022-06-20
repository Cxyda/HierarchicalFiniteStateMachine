namespace HFSM.Interfaces
{
    public interface IInitialInternal : IInitial, IPseudoStateInternal, IExitInternal
    {
    }
    public interface IInitial : IPseudoState, IExit
    {
    }
}