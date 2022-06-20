namespace HFSM.Interfaces
{
    public interface IExitInternal : IExit
    {
        void Exit();
    }
    public interface IExit
    {
        ITransition TransitionTo(IEnter targetState);
        void OnExit(ExitDelegate exitDelegate);
    }
}