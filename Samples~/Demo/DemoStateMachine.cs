using Packages.HFSM.Runtime.Impl;
using Packages.HFSM.Runtime.Impl.Data;
using Packages.HFSM.Runtime.Interfaces;
using UnityEngine;

public class DemoStateMachine : MonoBehaviour
{
    private readonly IStateMachineEvent _stateMachineEvent = StateMachineFactory.CreateEvent("evButtonTrigger");

    private IStateMachine _sm;
    private IDoActivity _doActivity;

    private void Awake()
    {
        _sm = StateMachineFactory.Create();
        _sm.LogLevel = LogLevel.Verbose;

        IState state = _sm.CreateState("FooState");
        IState triggerState = _sm.CreateState("TriggerState");
        IFinal finalState = _sm.CreateFinal();

        state.OnEnter(StateEnter);
        state.Nest(SetupNestedState);
        state.TransitionTo(triggerState).If(IsTrue);
        state.OnExit(StateExit);

        triggerState.OnEnter(() => Debug.Log("TriggerState Enter"));
        triggerState.TransitionTo(finalState).On(_stateMachineEvent);
        
        finalState.OnEnter(() => Debug.Log("Final Reached"));
    }

    private void SetupNestedState(IStateMachineTemplate template)
    {
        var nestedState = template.CreateState("NestedState");
        var finalState = template.CreateFinal("Final");
        
        nestedState.OnEnter(NestedEntry);
        nestedState.OnDo(NestedDo);
        nestedState.TransitionTo(finalState);
    }

    private void NestedDo(IDoActivity doActivity)
    {
        doActivity.Complete();
    }

    private void NestedEntry()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _doActivity.Complete();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            _sm.Trigger(_stateMachineEvent);
        }
    }
    private void DoSomething(IDoActivity doActivity)
    {
        _doActivity = doActivity;
    }

    private void StateEnter()
    {
    }
    private void StateExit()
    {
    }
    private bool IsTrue()
    {
        return true;
    }
    private void Start()
    {
        _sm.Start();
    }
}