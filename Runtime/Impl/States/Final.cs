using System.Collections.Generic;
using Packages.HFSM.Runtime.Interfaces;

namespace Packages.HFSM.Runtime.Impl.States
{
    public class Final : PseudoState, IFinalInternal
    {
        public Final(string name, int id) : base(new HashSet<EnterDelegate>(3), null, new HashSet<ITransitionInternal>(3))
        {
            Name = name;
            Id = id;
        }

        public void OnEnter(EnterDelegate enterDelegate)
        {
            onEnterDelegates.Add(enterDelegate);
        }

        public override bool Trigger(IStateMachineEvent @event)
        {
            return false;
        }
    }
}