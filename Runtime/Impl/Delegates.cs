using System;

namespace HFSM.Interfaces
{
    public delegate void EnterDelegate();
    public delegate void DoActivityDelegate(IDoActivity doActivity);
    public delegate void ExitDelegate();

    public delegate bool ConditionDelegate();

}