using Packages.HFSM.Runtime.Impl;

namespace Packages.HFSM.Runtime.Interfaces
{
    public interface IDoInternal : IDo
    {
        bool HasActivity();
        void Do();
    }

    public interface IDo
    {
        void OnDo(DoActivityDelegate doActivityDelegate);
    }
}