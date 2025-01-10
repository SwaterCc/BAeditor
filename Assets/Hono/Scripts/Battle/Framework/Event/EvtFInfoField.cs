using System.Runtime.CompilerServices;

namespace Hono.Scripts.Battle.Event
{
    public class EvtInfoField<T>
    {
        public readonly string Name;

        public EvtInfoField([CallerMemberName] string name = "")
        {
            Name = name;
        }
    }
}