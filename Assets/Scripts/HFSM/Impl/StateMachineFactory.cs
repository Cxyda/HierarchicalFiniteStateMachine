using System.IO;
using System.Runtime.CompilerServices;
using HFSM.Interfaces;

namespace HFSM.Impl
{
    public static class StateMachineFactory
    {
        public static IStateMachine Create([CallerFilePath] string name = "")
        {
            name = GetPrettyName(name);
            return new StateMachine(name);
        }

        public static IStateMachineEvent CreateEvent(string eventName)
        {
            return new StateMachineEvent(eventName);
        }

        private static string GetPrettyName(string name)
        {
            return Path.GetFileNameWithoutExtension(name);
        }
    }
}