using System.Collections.Generic;
using HFSM.Interfaces;

namespace HFSM.Impl.States
{
    public class Initial : PseudoState, IInitialInternal
    {
        internal Initial(string name, int id) : base(null, new HashSet<ExitDelegate>(3), new HashSet<ITransitionInternal>(3))
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