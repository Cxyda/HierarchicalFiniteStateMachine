namespace HFSM.Interfaces
{
    public interface IDoActivity
    {
        void Complete();
        IDoActivity Split();
        void Abort();
    }
}