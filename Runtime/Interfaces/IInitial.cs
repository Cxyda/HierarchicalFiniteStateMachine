namespace Packages.HFSM.Runtime.Interfaces
{
    public interface IInitialInternal : IInitial, IPseudoStateInternal, IExitInternal
    {
    }
    public interface IInitial : IPseudoState, IExit
    {
    }
}