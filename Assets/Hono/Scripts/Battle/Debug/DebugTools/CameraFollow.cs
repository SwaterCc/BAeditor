using System;
using Hono.Scripts.Battle.Core;
using UnityEngine;

namespace Hono.Scripts.Battle.DebugTools
{
    public class CameraFollow : MonoBehaviour
    {
        private void Update()
        {
            if (World.Current.PlayerControlUnit != null)
            {
                var player = World.Current.PlayerControlUnit;
                transform.position = player.UnitTransform.Pos + new Vector3(0, 20, -9);
                transform.rotation = Quaternion.Euler(61.8f, 0, 0);
            }
        }
    }
}