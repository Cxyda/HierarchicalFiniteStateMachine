using NUnit.Framework;
using Packages.HFSM.Runtime.Impl;
using Packages.HFSM.Runtime.Interfaces;
using UnityEngine;

namespace Packages.HFSM.Tests.Editor
{
    [TestFixture]
    public class HFSMTests
    {
        private IStateMachine _stateMachine;

        [SetUp]
        public void SetUp()
        {
        }
        [Test]
        public void NestedStateTest()
        {
            _stateMachine = StateMachineFactory.Create();
            SetupParentStateMachine(_stateMachine);

            _stateMachine.Start();
            void SetupParentStateMachine(IStateMachineTemplate template)
            {
                var parentState = template.CreateState("ParentState");
                var nextState = template.CreateState("ParentNextState");
                
                parentState.Nest(SetupNestedStateMachine);
                parentState.TransitionTo(nextState);
                
                nextState.OnEnter(() => Debug.Log("next State"));
            }
            void SetupNestedStateMachine(IStateMachineTemplate template)
            {
                var nested = template.CreateState("NestedState");
                var nestedFinal = template.CreateFinal("NestedFinal");

                nested.OnDo(NestedDo);
                nested.TransitionTo(nestedFinal);
            }
            void NestedDo(IDoActivity doactivity)
            {
                Debug.Log("Nested DoActivity");
                doactivity.Complete();
            }
        }
    }
}
