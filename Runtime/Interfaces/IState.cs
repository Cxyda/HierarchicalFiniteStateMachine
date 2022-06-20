using System;

namespace Packages.HFSM.Runtime.Interfaces
{
    public interface IStateInternal : IState, IPseudoStateInternal, IEnterInternal, IDoInternal, IExitInternal
    {
        event Action OnStateCompleteEvent;
        void Abort();
    }
    public interface IState : IPseudoState, IEnter, IDo, IExit
    {
        bool IsDone();
    }
}