using Packages.HFSM.Runtime.Impl.States;
using Packages.HFSM.Runtime.Interfaces;

namespace Packages.HFSM.Runtime.Impl
{
    internal static class StateFactory
    {
        private static int _stateCounter = 0;

        internal static IInitialInternal CreateInitial(string name)
        {
            return new Initial(name, ++_stateCounter);
        }

        internal static IStateInternal CreateState(string name)
        {
            return new State(name, ++_stateCounter);
        }

        internal static IFinalInternal CreateFinal(string name)
        {
            return new Final(name, ++_stateCounter);
        }
    }
}