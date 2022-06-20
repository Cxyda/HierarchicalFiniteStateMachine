namespace HFSM.Interfaces
{
    public interface IEnterInternal : IEnter
    {
        void Enter();
    }

    public interface IEnter
    {
        void OnEnter(EnterDelegate enterDelegate);
    }
}