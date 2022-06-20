namespace Packages.HFSM.Runtime.Interfaces
{
    public interface IDoActivity
    {
        void Complete();
        IDoActivity Split();
        void Abort();
    }
}