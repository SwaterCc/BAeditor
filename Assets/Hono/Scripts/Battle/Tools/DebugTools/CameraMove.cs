#region

using Cinemachine;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.Tools.DebugTools {
	public class CameraMove : MonoBehaviour {
		public Actor followActor;
		public FreeLookManager lookManager;
		public GameObject FreeLook;

		private Vector3 originPos;

		public void Awake() {
			originPos = transform.position;
		}

		public void Update() {
			var curBattleGround = BattleManager.CurBattle;
			if (curBattleGround == null) return;


			if (curBattleGround.RtInfo.CurRoundState == ERoundState.Ready && lookManager != null && FreeLook != null) {
				lookManager.enabled = true;
			}
			else {
				if (lookManager != null) {
					lookManager.enabled = false;
				}

				followActor = ActorManager.Instance.GetActor(curBattleGround.RtInfo.LeaderUid);
				if (followActor == null)
					return;
				if (FreeLook != null) {
					FreeLook.transform.localPosition = originPos + followActor.Pos;
				}
				else {
					Camera.main.transform.localPosition = originPos + followActor.Pos;
				}
			}
		}
	}
}