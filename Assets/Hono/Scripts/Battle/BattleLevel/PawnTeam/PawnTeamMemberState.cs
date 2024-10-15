using UnityEngine;

namespace Hono.Scripts.Battle
{
    public class PawnTeamMemberState
    {
        public int MemberIndex { get; }
        public PawnTeamState TeamState { get; }
        
        public bool IsLeader;
        
        private int _actorUid;

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

        public void CreateMember()
        {
            if (_actorConfigId == -1)
            {
                return;
            }
            
            _actorUid = ActorManager.Instance.CreateActor(EActorType.Pawn, _actorConfigId, (actor) =>
            {
                actor.OnModelLoadFinish += (_) => IsLoadFinish = true;
                actor.OnTickCallBack += onPawnTick;
                actor.OnDestroyCallBack += onPawnDead;

                actor.SetAttr(ELogicAttr.AttrPosition, TeamState.GetMemberTeamPos(MemberIndex), false);
                actor.SetAttr(ELogicAttr.AttrRot, Quaternion.identity, false);
                BattleManager.CurrentBattleGround.RuntimeInfo.AddFactionActorCount(actor.GetAttr<int>(ELogicAttr.AttrFaction));
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
            
            if (BattleManager.CurrentBattleGround != null)
            {
                BattleManager.CurrentBattleGround.RuntimeInfo.OnActorDead(actor);
            }
        }

        public void SetPlayerControlFlag(bool isControl)
        {
            var actor = ActorManager.Instance.GetActor(_actorUid);
            actor.IsPlayerControl = isControl;
            if (isControl)
            {
                BattleManager.CurrentBattleGround.RuntimeInfo.LeaderUid = actor.Uid;
            }
        }
    }
}