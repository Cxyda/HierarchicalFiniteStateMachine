using System;

namespace HFSM.Interfaces
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