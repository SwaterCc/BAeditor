#region

using UnityEngine;

#endregion

namespace Hono.Scripts.Battle {
	public class PawnTeamMemberState {
		public int MemberIndex { get; }
		public PawnTeamState TeamState { get; }

		public bool IsLeader;

		private int _actorUid;
		public int ActorUid => _actorUid;

		private readonly int _actorConfigId;

		public EPawnTeamMemberStateType CurStateType;

		public Vector3 Pos { get; private set; }

		public Quaternion Rot { get; private set; }

		public PawnTeamMemberState(PawnTeamState teamState, int memberIndex, int actorConfigId) {
			TeamState = teamState;
			MemberIndex = memberIndex;

			_actorConfigId = actorConfigId;
		}

		public void CreateMember() {
			CurStateType = _actorConfigId <= 0 ? EPawnTeamMemberStateType.Empty : EPawnTeamMemberStateType.Normal;
			if (_actorConfigId == -1) {
				return;
			}

		
		}

		private void onPawnTick(Actor actor) {
			Pos = actor.Pos;
			Rot = actor.Rot;

			actor.TargetPos= TeamState.GetMemberTeamPos(MemberIndex);
		}

		private void onPawnDead(Actor actor) {
			CurStateType = EPawnTeamMemberStateType.Dead;

			if (IsLeader) {
				IsLeader = false;
				actor.IsPlayerControl = false;
				TeamState.PassingLeader();
			}

			if (BattleManager.CurBattle != null) {
				BattleManager.CurBattle.RtInfo.OnActorDead(actor);
			}
		}

		public void SetPlayerControlFlag(bool isControl) {
			var actor = ActorManager.Instance.GetActor(_actorUid);
			actor.IsPlayerControl = isControl;
			if (isControl) {
				BattleManager.CurBattle.RtInfo.LeaderUid = actor.Uid;
			}
		}
	}
}