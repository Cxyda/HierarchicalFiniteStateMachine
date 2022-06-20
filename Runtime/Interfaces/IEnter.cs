using Packages.HFSM.Runtime.Impl;

namespace Packages.HFSM.Runtime.Interfaces
{
    public interface IEnterInternal : IEnter
    {
        void Enter();
    }

    public interface IEnter
    {
        void OnEnter(EnterDelegate enterDelegate);
    }
}