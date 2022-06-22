using System;
using Packages.HFSM.Runtime.Impl;

namespace Packages.HFSM.Runtime.Interfaces
{
    public interface IStateInternal : IState, IPseudoStateInternal, IEnterInternal, IDoInternal, IExitInternal
    {
        event Action OnStateCompleteEvent;
        IPseudoStateInternal CurrentState { get; }
        void Abort();
        bool IsDone();
        bool IsCompositeState();
        void MarkStateAsDone();
    }
    public interface IState : IPseudoState, IEnter, IDo, IExit
    {
        void Nest(StateMachineSetupDelegate stateMachineSetup);
    }
}