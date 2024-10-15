using System;
using UnityEngine;

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
            var curBattleGround = BattleManager.CurrentBattleGround;
            if (curBattleGround == null) return;
            
            followActor = ActorManager.Instance.GetActor(curBattleGround.RuntimeInfo.LeaderUid);
            if (followActor == null) return;
            
            transform.localPosition = originPos + followActor.Pos;
        }
    }
}