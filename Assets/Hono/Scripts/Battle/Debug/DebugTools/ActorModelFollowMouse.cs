#region

using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.Tools.DebugTools
{
    public class ActorModelFollowMouse : MonoBehaviour
    {
        private Camera _mainCamera;
        public Vector3 WorldPos { get; private set; }

        private void OnEnable()
        {
            _mainCamera = Camera.main;
        }

        public void Update()
        {
            /*var mouseScreenPosition = Input.mousePosition;
            WorldPos = _mainCamera
                .ScreenPointToRay(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, _mainCamera.nearClipPlane));
            transform.position = WorldPos;*/
        }
    }
}