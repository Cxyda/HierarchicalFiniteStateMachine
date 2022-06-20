namespace HFSM.Interfaces
{
    public interface IFinalInternal : IFinal, IPseudoStateInternal, IEnterInternal
    {
    }
    public interface IFinal : IPseudoState, IEnter
    {
    }
}