using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public class PawnTeamState
    {
        private readonly List<PawnTeamMemberState> _memberStates;
        private int _curLeaderIndex;
        private PawnTeamMemberState _leader;

        private float _duration;
        private const float CenterPosUpdateInterval = 0.5f;
        private const float TeamRadius = 1.618f;
        private readonly int _teamMemberCount;
        
        public readonly List<Vector3> MemberDirection;
        
        public int TeamIndex { get; }
        public Vector3 CenterPos { get; private set; }
        public Quaternion CenterRot { get; private set; }
        public int LeaderUid { get => _leader == null ? -1 : _leader.ActorUid; }

        public PawnTeamState(int teamIndex, int teamMemberCount, List<int> memberConfigIds)
        {
            _teamMemberCount = teamMemberCount;
            MemberDirection = new()
            {
                Vector3.forward,
                Vector3.right,
                Vector3.back,
                Vector3.left,
            };
            
            TeamIndex = teamIndex;
            
            _memberStates = new List<PawnTeamMemberState>()
            {
                new(this,0,memberConfigIds[0]),
                new(this,1,memberConfigIds[1]),
                new(this,2,memberConfigIds[2]),
                new(this,3,memberConfigIds[3]),
            };

            PassingLeader();
        }

        /// <summary>
        /// 自动传位
        /// </summary>
        public void PassingLeader()
        {
            //顺位寻找下一个队长
            for (; _curLeaderIndex < BattleConstValue.TeamMemberMaxCount; ++_curLeaderIndex)
            {
                if (_memberStates[_curLeaderIndex].CurStateType != EPawnTeamMemberStateType.Normal) continue;
                _memberStates[_curLeaderIndex].IsLeader = true;
                _leader = _memberStates[_curLeaderIndex];
                break;
            }
        }

        public void CreateTeam(Vector3 teamCenter, Quaternion teamRot)
        {
            CenterPos = teamCenter;
            foreach (var state in _memberStates)
            {
                state.CreateMember();
            }
        }
        
        public void OnTick(float dt)
        {
	        updateCenterPos();
            if (_duration > CenterPosUpdateInterval)
            {
                
                _duration = 0;
            }

            _duration += dt;
        }

        public void OnPlayerControlChange(bool isControl)
        {
            _leader.SetPlayerControlFlag(isControl);
        }
        
        private void updateCenterPos()
        {
            CenterPos = _leader.Pos + _leader.Rot * (-MemberDirection[_curLeaderIndex] * TeamRadius);
            CenterRot = _leader.Rot;
        }

        public Vector3 GetMemberTeamPos(int memberIndex)
        {
           return CenterPos + CenterRot * (MemberDirection[memberIndex] * TeamRadius);
        }
    }
}