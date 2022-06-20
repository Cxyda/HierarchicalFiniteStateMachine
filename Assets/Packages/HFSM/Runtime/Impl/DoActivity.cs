using System;
using HFSM.Interfaces;

namespace HFSM.Impl
{
    public class DoActivity : IDoActivity
    {
        private readonly Action _onCompleteAction;
        private ushort _completedActivitiesCounter;
        private ushort _activityCounter;

        public DoActivity(Action completeAction)
        {
            _onCompleteAction = completeAction;
            _completedActivitiesCounter = 0;
            _activityCounter = 1;
        }

        public void Complete()
        {
            if (_activityCounter == 0)
            {
                return;
            }
            _completedActivitiesCounter++;
            if (_completedActivitiesCounter >= _activityCounter)
            {
                _completedActivitiesCounter = 0;
                _onCompleteAction?.Invoke();
            }
        }

        public IDoActivity Split()
        {
            _activityCounter++;
            return this;
        }

        public void Abort()
        {
            _completedActivitiesCounter = 0;
        }
    }
}