using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Tools;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public class PawnGroupState
    {
        public class MemberState
        {
            public int MemberIdx { get; }
            private bool _isLeader;
            public Vector3 GroupPos;
            public bool IsLeader
            {
                get => _isLeader;
                set
                {
                    _isLeader = value;
                    if (IsLeader)
                    {
                        StateType = EPawnGroupMemberStateType.Leader;
                        _groupState.Leader = this;
                    }
                }
            }

            public EPawnGroupMemberStateType StateType;
            public int MemberActorUid;
            public int MemberActorConfig;
            private readonly PawnGroupState _groupState;
            private ActorStateChecker _stateChecker;

            public MemberState(PawnGroupState groupState, int memberIdx)
            {
                _groupState = groupState;
                MemberIdx = memberIdx;
                MemberActorUid = -1;
                MemberActorConfig = -1;
            }

            public void Init(int actorUid,int configId,EPawnGroupMemberStateType stateType)
            {
                StateType = stateType;
                MemberActorConfig = configId;
                MemberActorUid = actorUid;
                _stateChecker = new ActorStateChecker(EBattleEventType.OnActorDead, actorUid, (eventInfo) =>
                {
                    StateType = EPawnGroupMemberStateType.Dead;
                    IsLeader = false;
                    _groupState.PassLeader(MemberIdx);
                });
                BattleEventManager.Instance.Register(_stateChecker);
            }

            public Vector3 GetCenterPos()
            {
                var leaderActor = ActorManager.Instance.GetActor(MemberActorUid);
                var centerPos = leaderActor.Pos;
                if (MemberIdx == 0)
                {
                    centerPos = leaderActor.Pos + leaderActor.Rot * (Vector3.back * _groupState._groupRadius);
                }
                if (MemberIdx == 1)
                {
                    centerPos = leaderActor.Pos + leaderActor.Rot * (Vector3.right * _groupState._groupRadius);
                }
                if (MemberIdx == 2)
                {
                    centerPos = leaderActor.Pos + leaderActor.Rot * (Vector3.forward * _groupState._groupRadius);
                }
                if (MemberIdx == 3)
                {
                    centerPos = leaderActor.Pos + leaderActor.Rot * (Vector3.left * _groupState._groupRadius);
                }

                return centerPos;
            }

            public void SetGroupPos(Vector3 center)
            {
                
            }

            public static explicit operator int(MemberState a)
            {
                if (a.StateType == EPawnGroupMemberStateType.Dead || a.StateType == EPawnGroupMemberStateType.Empty)
                    return 0;
                else
                    return 1;
            }

            public static int operator +(MemberState a, MemberState b)
            {
                return (int)a + (int)b;
            }

            public static int operator +(int a, MemberState b)
            {
                return a + (int)b;
            }

            public static int operator +(MemberState a, int b)
            {
                return (int)a + b;
            }
        }

        public int GroupIndex { get; }

        public MemberState Leader { get; private set; }

        public bool IsAllDead { get; private set; }

        private readonly MemberState _member0;
        private readonly MemberState _member1;
        private readonly MemberState _member2;
        private readonly MemberState _member3;
        
        private Actor _leaderActor;
        private float _groupRadius = 1.5f;
        
        public PawnGroupState(int groupIndex)
        {
            GroupIndex = groupIndex;
            _member0 = new MemberState(this, 0);
            _member1 = new MemberState(this, 1);
            _member2 = new MemberState(this, 2);
            _member3 = new MemberState(this, 3);
            IsAllDead = false;
        }

        public int MemberCount => _member0 + _member1 + _member2 + _member3;

        public MemberState this[int idx]
        {
            get
            {
                idx = Mathf.Clamp(idx, 0, BattleConstValue.PawnGroupMemberMaxCount - 1);
                switch (idx)
                {
                    case 0:
                        return _member0;
                    case 1:
                        return _member1;
                    case 2:
                        return _member2;
                    case 3:
                        return _member3;
                }

                throw new ArgumentOutOfRangeException("超出队伍成员长度！");
            }
        }

        public void PassLeader(int memberIdx)
        {
            _leaderActor = null;
            while (memberIdx++ < BattleConstValue.PawnGroupMemberMaxCount)
            {
                if (this[memberIdx].StateType != EPawnGroupMemberStateType.Normal) continue;
                this[memberIdx].IsLeader = true;
                break;
            }
            
            IsAllDead = true;
        }
        
        public void OnTick(float dt)
        {
            if(IsAllDead) return;

            if (Leader == null) return;
           
            _leaderActor ??= ActorManager.Instance.GetActor(Leader.MemberActorUid);

            if (_leaderActor != null)
            {
                var centerPos = Vector3.zero;
                var centerRot = Quaternion.identity;
               
            }
        }
    }

    public class PawnGroupManager : Singleton<PawnGroupManager>
    {
        private PawnGroupState _curControlGroup;
        public PawnGroupState CurControlGroup => _curControlGroup;
        private readonly Dictionary<int, PawnGroupState> _groupStates = new(BattleConstValue.PawnGroupMaxCount);

        public void Init()
        {
            _groupStates.Clear();
        }

        public void AddGroup(int idx, PawnGroupState groupState)
        {
            if (!_groupStates.TryAdd(idx, groupState))
            {
                Debug.LogError("重复添加队伍");
                
                return;
            }

            if (_curControlGroup == null)
            {
                ChangeControlGroup(idx);
            }
        }

        public void Tick(float dt)
        {
            foreach (var pGroupState in _groupStates)
            {
                pGroupState.Value.OnTick(dt);
            }

            if (_curControlGroup.IsAllDead)
            {
                foreach (var pGroupState in _groupStates)
                {
                    if (!pGroupState.Value.IsAllDead)
                    {
                        ChangeControlGroup(pGroupState.Key);
                        break;
                    }
                }
            }
        }

        public void ChangeControlGroup(int groupIdx)
        {
            if (_curControlGroup != null && _curControlGroup.GroupIndex == groupIdx) return;

            if (_groupStates.TryGetValue(groupIdx, out var groupState) && (!groupState.IsAllDead))
            {
                if (_curControlGroup != null)
                {
                    //恢复自动输入
                    if (ActorManager.Instance.TryGetActor(groupState.Leader.MemberActorUid, out var beforeControl))
                    {
                        var pawnLogic = (PawnLogic)beforeControl.Logic;
                        pawnLogic.ChangeInputToAuto();
                    }
                }
                
                _curControlGroup = groupState;
                //切换相机到队长身上
                //改变队长的操作方式
                if (ActorManager.Instance.TryGetActor(groupState.Leader.MemberActorUid, out var pawn))
                {
                    var pawnLogic = (PawnLogic)pawn.Logic;
                    pawnLogic.ChangeInputToManual();
                }
            }
            else
            {
                Debug.LogError($"尝试切换到不存在的队伍 groupID {groupIdx}");
            }
        }
    }
}