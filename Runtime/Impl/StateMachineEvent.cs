using Packages.HFSM.Runtime.Interfaces;

namespace Packages.HFSM.Runtime.Impl
{
    public sealed class StateMachineEvent : IStateMachineEventInternal
    {
        public string Name => _name;
        public bool HasBeenInvoked => _hasBeenInvoked;

        private readonly string _name;
        private bool _hasBeenInvoked;

        private StateMachineEvent()
        {
        }

        internal StateMachineEvent(string eventName)
        {
            _name = eventName;
        }

        private bool Equals(StateMachineEvent other)
        {
            return _name == other._name;
        }

        public override bool Equals(object obj)
        {
            return obj is StateMachineEvent other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (_name != null ? _name.GetHashCode() : 0);
        }

        public void Invoke()
        {
            _hasBeenInvoked = true;
        }

        public void Reset()
        {
            _hasBeenInvoked = false;
        }

    }
}