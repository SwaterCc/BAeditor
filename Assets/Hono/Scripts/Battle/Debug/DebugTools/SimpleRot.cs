#region

using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.Tools.DebugTools
{
    public class SimpleRot : MonoBehaviour
    {
        public void Update()
        {
            transform.Rotate(Vector3.up, 180 * Time.deltaTime);
        }
    }
}