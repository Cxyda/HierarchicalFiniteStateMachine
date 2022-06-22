using System;
using System.IO;
using System.Runtime.CompilerServices;
using Packages.HFSM.Runtime.Interfaces;

namespace Packages.HFSM.Runtime.Impl
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