using UnityEngine;

namespace Hono.Scripts.Battle
{
    public class NoInput : IInputHandle
    {
        public Vector3 MoveInputValue => Vector3.zero;
    }
}