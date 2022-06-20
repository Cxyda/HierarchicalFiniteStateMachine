using HFSM.Impl;
using HFSM.Interfaces;
using UnityEngine;

public class DemoStateMachine : MonoBehaviour
{
    private readonly IStateMachineEvent _stateMachineEvent = StateMachineFactory.CreateEvent("evButtonTrigger");

    private IStateMachine _sm;
    private IDoActivity _doActivity;

    private void Awake()
    {
        _sm = StateMachineFactory.Create("Test");

        IInitial initial = _sm.CreateInitial();
        IState state = _sm.CreateState("FooState");
        IState triggerState = _sm.CreateState("TriggerState");
        IFinal finalState = _sm.CreateFinal();

        initial.OnExit(InitialExit);
        initial.TransitionTo(state);
        
        state.OnEnter(StateEnter);
        state.OnDo(DoSomething);
        state.TransitionTo(triggerState).If(IsTrue);
        state.OnExit(StateExit);

        triggerState.OnEnter(() => Debug.Log("Foooo"));
        triggerState.TransitionTo(finalState).When(_stateMachineEvent);
        
        finalState.OnEnter(() => Debug.Log("Final Reached"));
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
        Debug.Log("Do Activity");
        _doActivity = doActivity;
    }

    private void InitialExit()
    {
        Debug.Log("Initial Exit");
    }
    private void StateEnter()
    {
        Debug.Log("State Enter");
    }
    private void StateExit()
    {
        Debug.Log("State Exit");
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