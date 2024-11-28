#region

using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    /// <summary>
    ///     自动输入
    ///     遍历主动技能
    ///     如果有可以释放的则释放技能
    ///     如果没有则向仇恨目标移动
    /// </summary>
    public class AutoInput : ActorInput
    {
        public AutoInput(ActorLogic logic) : base(logic) { }

        protected ActorLogic.SkillComp _skillComp;

        private float _useSkillDt;
        private Vector3 _beforeTargetPos;
        private Path _path;
        private bool _hasPath;
        private bool _pathBuildFinish;
        private float _pathUpdateDuration;
        private int _pathIdx;
        private float _hateFollowRange;
        private List<Vector3> _wayPoint = new(16);

        protected override void onInit()
        {
            _beforeTargetPos = Vector3.zero;
            Logic.TryGetComponent(out _skillComp);
            var wayPoint = (List<Vector3>)Logic.Self.Variables.Get("WayPoints");
            if (wayPoint is { Count: > 0 })
            {
                _wayPoint.AddRange(wayPoint);
            }

            if (Logic is MonsterLogic monsterLogic)
            {
                _hateFollowRange = monsterLogic.MonsterConfig.SearchRadiu;
            }
        }


        protected override void onTick(float dt)
        {
            AutoMove();

            _pathUpdateDuration += dt;
            _useSkillDt += dt;
            if (!(_useSkillDt > 0.2f)) return;

            AutoUseSkill();
            _useSkillDt = 0;
        }


        protected virtual void AutoMove()
        {
            if (Logic.CurState() == EActorStateType.Attack) return;


            if (!TryGetTargetPos(out var targetPos))
            {
                MoveInputValue = Vector3.zero;
                return;
            }

            if (_pathUpdateDuration > 1f)
            {
                //如果目标改变了，则立刻重新生成路径
                _pathUpdateDuration = 0;
                if (Logic.Self.ModelController.Model.TryGetComponent<Seeker>(out var seeker))
                {
                    seeker.StartPath(Logic.Self.Pos, targetPos, FindPathFinish);
                }

                _beforeTargetPos = targetPos;
            }

            if (_hasPath)
            {
                PathMove();
            }
        }

        private void FindPathFinish(Path path)
        {
            _hasPath = !path.error && path.vectorPath.Count > 0;
            if (_hasPath)
            {
                Debug.Log($"[FindPathFinish] Actor {Logic.Uid} FindNewPath to NewPoint {path.vectorPath[0]}");
                _pathIdx = 0;
                _path = path;
            }
            else
            {
                Debug.Log($"[FindPathFinish] Actor {Logic.Uid} Not FindNewPath !");
                MoveInputValue = Vector3.zero;
            }
        }

        //优先向仇恨目标
        //其次向路点移动
        //技能期间不会移动
        //玩家角色除了手操角色以外
        protected bool TryGetTargetPos(out Vector3 targetPos)
        {
            var curPos = Logic.Self.Pos;
            var disPrecision = 0.1f;
            bool hasMove = false;
            if (TryGetHateTarget(out var moveTargetPos))
            {
                disPrecision = _hateFollowRange > 0 ? _hateFollowRange : 2;
                hasMove = true;
            }
            else if (TryGetWayPoint(curPos, out moveTargetPos))
            {
                hasMove = true;
            }
            else if (TryGetOriginPos(out moveTargetPos))
            {
                disPrecision = 0.2f;
                hasMove = true;
            }

            if (hasMove && Vector3.Distance(moveTargetPos, curPos) > disPrecision)
            {
                targetPos = moveTargetPos;
                return true;
            }

            targetPos = Vector3.zero;
            return false;
        }

        private void PathMove()
        {
            if (_path.vectorPath.Count == 0 || _pathIdx >= _path.vectorPath.Count) return;

            var targetPos = _path.vectorPath[_pathIdx];
            var curPos = Logic.Self.Pos;

            if (Vector3.Distance(curPos, targetPos) <= 2f)
            {
                //到达寻路点0.1米左右就算到达
                ++_pathIdx;
                if (_pathIdx == _path.vectorPath.Count)
                {
                    MoveInputValue = Vector3.zero;
                    _hasPath = false;
                    return;
                }
            }

            var inputDir = (targetPos - curPos).normalized;
            inputDir.y = 0;
            MoveInputValue = inputDir;
        }


        #region 移动方法

        /// <summary>
        ///     路点移动
        /// </summary>
        /// <param name="curPos"></param>
        /// <param name="targetPos"></param>
        /// <returns></returns>
        protected bool TryGetWayPoint(Vector3 curPos, out Vector3 targetPos)
        {
            targetPos = Vector3.zero;

            if (_wayPoint is { Count: > 0 })
            {
                targetPos = _wayPoint.First();
                if (Vector3.Distance(curPos, targetPos) < 2f)
                {
                    Logic.Self.TargetPos = targetPos;
                    _wayPoint.RemoveAt(0);
                    if (_wayPoint.Count == 0)
                    {
                        return false;
                    }

                    targetPos = _wayPoint[0];
                    return true;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        ///     向仇恨目标移动
        /// </summary>
        /// <param name="targetPos"></param>
        /// <returns></returns>
        protected bool TryGetHateTarget(out Vector3 targetPos)
        {
            targetPos = Vector3.zero;

            var hateTargetUid = Logic.Self.GetAttr(EAttrType.AttrHateTargetUid);

            if (hateTargetUid <= 0)
            {
                return false;
            }

            if (!ActorManager.Instance.TryGetActor(hateTargetUid, out var hateTarget))
                return false;

            targetPos = hateTarget.Pos;
            return true;
        }

        /// <summary>
        ///     尝试回归原点
        /// </summary>
        /// <returns></returns>
        protected bool TryGetOriginPos(out Vector3 targetPos)
        {
            targetPos = Logic.Self.TargetPos;
            return true;
        }

        #endregion

        #region 自动释放技能

        protected virtual void AutoUseSkill()
        {
            if (_skillComp == null) return;

            if (Logic.CurState() == EActorStateType.Attack) return;

            foreach (var pSkill in _skillComp.Skills)
            {
                var skill = pSkill.Value;
                if (!skill.IsEnable)
                    continue;
                if (skill.SkillData.skillType == ESkillType.PassiveSkill)
                    continue;
                if (skill.SkillData.skillType == ESkillType.UltimateSkill &&
                    !BattleManager.CurBattle.RtInfo.OpenAutoUlt)
                    continue;
                if (_skillComp.TryUseSkill(skill.Id))
                {
                    return;
                }
            }
        }

        #endregion
    }
}