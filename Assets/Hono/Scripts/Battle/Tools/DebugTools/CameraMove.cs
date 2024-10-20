//using Cinemachine;
using System;
using UnityEngine;

namespace Hono.Scripts.Battle.Tools.DebugTools
{
    public class CameraMove : MonoBehaviour
    {
	    private Vector3 originPos;
	    public void Awake()
	    {
		    originPos = transform.position;
	    }

	    private void Update()
	    {
		    var curBattleGround = BattleManager.CurrentBattleGround;
		    if (curBattleGround == null) return;
		    var followActor = ActorManager.Instance.GetActor(curBattleGround.RuntimeInfo.LeaderUid);
		    Camera.main.transform.localPosition = originPos + followActor.Pos;
	    }

	    /*public Actor followActor;
		public FreeLookManager lookManager;
		public GameObject FreeLook;

        private Vector3 originPos;
        
        public void Awake()
        {
            originPos = transform.position;
            
        }

        public void Update()
        {
            var curBattleGround = BattleManager.CurrentBattleGround;
            if (curBattleGround == null) return;
         
			
			if(curBattleGround.RuntimeInfo.CurRoundState == ERoundState.Ready && lookManager != null && FreeLook != null ) {
				lookManager.enabled = true;
			}
			else {
				if (lookManager != null) {
					lookManager.enabled = false;
				}
			
				followActor = ActorManager.Instance.GetActor(curBattleGround.RuntimeInfo.LeaderUid);
				if (followActor == null)
					return;
				if (FreeLook != null) {
					FreeLook.transform.localPosition = originPos + followActor.Pos;
				}
				else {
					Camera.main.transform.localPosition = originPos + followActor.Pos;
				}
			}
        }*/
    }
}