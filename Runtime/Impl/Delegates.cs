using Packages.HFSM.Runtime.Interfaces;

namespace Packages.HFSM.Runtime.Impl
{
    public delegate void EnterDelegate();
    public delegate void DoActivityDelegate(IDoActivity doActivity);
    public delegate void ExitDelegate();

    public delegate bool ConditionDelegate();

}