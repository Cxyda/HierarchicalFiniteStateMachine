namespace Packages.HFSM.Runtime.Interfaces
{
    public interface IFinalInternal : IFinal, IPseudoStateInternal, IEnterInternal
    {
    }
    public interface IFinal : IPseudoState, IEnter
    {
    }
}