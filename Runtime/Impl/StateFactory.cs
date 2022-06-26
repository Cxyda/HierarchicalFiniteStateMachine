using System.Diagnostics.CodeAnalysis;
using Packages.HFSM.Runtime.Impl.States;
using Packages.HFSM.Runtime.Impl.Utils;
using Packages.HFSM.Runtime.Interfaces;

namespace Packages.HFSM.Runtime.Impl
{
    internal static class StateFactory
    {
        private static int _stateCounter = 0;

        internal static IInitialInternal CreateInitial(string name, [NotNull]ILogger logger)
        {
            return new Initial( name, ++_stateCounter, logger);
        }

        internal static IStateInternal CreateState(IStateMachine stateMachine, IStateInternal parentState,
            string name, [NotNull] ILogger logger)
        {
            return new State(stateMachine, parentState, name, ++_stateCounter, logger);
        }

        internal static IFinalInternal CreateFinal(string name, [NotNull]ILogger logger)
        {
            return new Final(name, ++_stateCounter, logger);
        }
    }
}