#region

using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.Tools.DebugTools
{
    public class CameraMove : MonoBehaviour
    {
        public Actor followActor;
        private Vector3 originPos;

        public void Awake()
        {
            originPos = transform.position;
        }

        public void Update()
        {
            var curBattleGround = BattleManager.CurBattle;
            if (curBattleGround == null) return;
            
        }
    }
}