using System;
using NUnit.Framework;
using Packages.HFSM.Runtime.Impl;
using Packages.HFSM.Runtime.Interfaces;

namespace Packages.HFSM.Tests.Editor
{
    [TestFixture]
    public class HFSMTests
    {
        private static string _enterLogMessage = "Enter";
        private static string _doLogMessage = "Do";
        private static string _exitLogMessage = "Exit";
        private static string _finalLogMessage = "FINAL";

        private IStateMachine _stateMachine;

        private string _traceString;
        private bool _condition;
        [SetUp]
        public void SetUp()
        {
            _traceString = String.Empty;
        }

        [Test]
        public void SimpleStateMachine_WhenNotStarted_DoesNotExecute()
        {
            _stateMachine = StateMachineFactory.Create();
            SetupSimpleStateMachine(_stateMachine);
            Assert.IsFalse(_stateMachine.IsRunning);
            Assert.AreEqual(string.Empty, _traceString);
        }
        
        [Test]
        public void SimpleStateMachine_WhenStarted_Works()
        {
            _stateMachine = StateMachineFactory.Create();
            SetupSimpleStateMachine(_stateMachine);
            _stateMachine.Start();
            Assert.IsTrue(_stateMachine.IsRunning);

            Assert.AreEqual($"{_enterLogMessage}->{_doLogMessage}->{_exitLogMessage}->{_finalLogMessage}", _traceString);
        }

        [Test]
        public void SimpleStateMachine_WhenStopped_Stops()
        {
            _stateMachine = StateMachineFactory.Create();
            SetupSimpleStateMachine(_stateMachine);
            _stateMachine.Start();
            
            Assert.IsTrue(_stateMachine.IsRunning);
            _stateMachine.Stop();
            Assert.IsFalse(_stateMachine.IsRunning);
            Assert.AreEqual($"{_enterLogMessage}->{_doLogMessage}->{_exitLogMessage}->{_finalLogMessage}", _traceString);
        }

        [Test]
        public void ConditionalStateMachine_WorksAsExpected()
        {
            _condition = true;
            _stateMachine = StateMachineFactory.Create();
            SetupConditionalStateMachine(_stateMachine);
            _stateMachine.Start();
            
            Assert.IsTrue(_stateMachine.IsRunning);
            Assert.AreEqual($"State->TargetState", _traceString);
        }
        
        
        private void SetupConditionalStateMachine(IStateMachineTemplate template)
        {
            var targetStateName = "TargetState";
            var alternativeStateName = "AlternativeState";

            var state = template.CreateState();
            var targetState = template.CreateState(targetStateName);
            var alternativeState = template.CreateState(alternativeStateName);

            state.OnEnter(() => _traceString += "State->");
            state.TransitionTo(alternativeState).If(() => !_condition);
            state.TransitionTo(targetState).If(() => _condition);
            
            targetState.OnEnter(() => _traceString += targetStateName);
            alternativeState.OnEnter(() => _traceString += alternativeStateName);
        }

        private void SetupSimpleStateMachine(IStateMachineTemplate template)
        {
            var state = template.CreateState();
            var final = template.CreateFinal();

            state.OnEnter(() => _traceString += $"{_enterLogMessage}->");
            state.OnDo(activity =>
            {
                _traceString += $"{_doLogMessage}->";
                activity.Complete();
            });
            state.OnExit(() => _traceString += $"{_exitLogMessage}->");
            state.TransitionTo(final);
            
            final.OnEnter(() => _traceString += _finalLogMessage);
        }

        [Test]
        public void NestedStateTest()
        {
            var abortEvent = StateMachineFactory.CreateEvent("AbortEvent");
            _stateMachine = StateMachineFactory.Create();
            SetupParentStateMachine(_stateMachine);

            _stateMachine.Start();
            Assert.IsTrue(_stateMachine.IsRunning);
            Assert.AreEqual($"NestedDoExit->NextState", _traceString);

            void SetupParentStateMachine(IStateMachineTemplate template)
            {
                var parentState = template.CreateState("ParentState");
                var nextState = template.CreateState("ParentNextState");
                
                parentState.Nest(SetupNestedStateMachine);
                parentState.TransitionTo(nextState).On(abortEvent);
                
                nextState.OnEnter(() => _traceString+="NextState");
            }
            void SetupNestedStateMachine(IStateMachineTemplate template)
            {
                var nested = template.CreateState("NestedState");
                var nestedFinal = template.CreateFinal("NestedFinal");

                nested.OnDo(NestedDo);
                nested.OnExit(() => _traceString+= "NestedDoExit->");

                nestedFinal.OnEnter(() => _traceString += "Should not be executed");

                nested.TransitionTo(nestedFinal);
            }
            void NestedDo(IDoActivity doactivity)
            {
                _stateMachine.Trigger(abortEvent);

                // never completes
            }
        }
    }
}
