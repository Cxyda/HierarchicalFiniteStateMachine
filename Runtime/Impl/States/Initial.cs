using System.Collections.Generic;
using Packages.HFSM.Runtime.Impl.Utils;
using Packages.HFSM.Runtime.Interfaces;

namespace Packages.HFSM.Runtime.Impl.States
{
    public class Initial : PseudoState, IInitialInternal
    {
        internal Initial(string name, int id, ILogger logger) : base(null, new HashSet<ExitDelegate>(3), new HashSet<ITransitionInternal>(3), logger)
        {
            Id = id;
            Name = name;
        }

        public void OnExit(ExitDelegate exitDelegate)
        {
            onExitDelegates.Add(exitDelegate);
        }
        
        public override bool Trigger(IStateMachineEvent @event)
        {
            return false;
        }
    }
}