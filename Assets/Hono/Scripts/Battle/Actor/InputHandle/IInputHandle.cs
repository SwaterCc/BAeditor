using UnityEngine;

namespace Hono.Scripts.Battle
{
    public interface IInputHandle
    {
        public Vector3 MoveInputValue { get; }
    }
}