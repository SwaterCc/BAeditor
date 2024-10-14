using UnityEngine;

namespace Hono.Scripts.Battle
{
    public class PawnTeamMemberState
    {
        public int MemberIndex { get; }
        public PawnTeamState TeamState { get; }
        
        public bool IsLeader;
        
        public int _actorUid;

        private readonly int _actorConfigId;

        public bool IsLoadFinish { get; private set; }

        public EPawnTeamMemberStateType CurStateType;

        public Vector3 Pos { get; private set; }

        public Quaternion Rot {get; private set; }

        public PawnTeamMemberState(PawnTeamState teamState,int memberIndex,int actorConfigId)
        {
            TeamState = teamState;
            MemberIndex = memberIndex;
            
            _actorConfigId = actorConfigId;
            CurStateType = _actorConfigId <= 0 ? EPawnTeamMemberStateType.Empty : EPawnTeamMemberStateType.Normal;
        }

        public void CreateMember(Vector3 centerPos)
        {
            _actorUid = ActorManager.Instance.CreateActor(EActorType.Pawn, _actorConfigId, (actor) =>
            {
                actor.OnModelLoadFinish += (_) => IsLoadFinish = true;
                actor.OnTickCallBack += onPawnTick;
                actor.OnDestroyCallBack += onPawnDead;
            });
        }
        
        private void onPawnTick(Actor actor)
        {
            Pos = actor.Pos;
            Rot = actor.Rot;

            actor.SetAttr(ELogicAttr.AttrOriginPos, TeamState.GetMemberTeamPos(MemberIndex), false);
        }
        
        private void onPawnDead(Actor actor)
        {
            if (IsLeader)
            {
                IsLeader = false;
                TeamState.PassingLeader();
            }
            CurStateType = EPawnTeamMemberStateType.Dead;
        }

        public void SetPlayerControlFlag(bool isControl)
        {
            var actor = ActorManager.Instance.GetActor(_actorUid);
            actor.IsPlayerControl = isControl;
        }
    }
}