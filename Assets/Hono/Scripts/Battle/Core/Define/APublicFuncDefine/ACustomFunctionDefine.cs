#region

using System.Collections.Generic;
using Hono.Scripts.Battle.Core;
using Hono.Scripts.Battle.Tools;
using Hono.Scripts.Battle.Tools.CustomAttribute;
using UnityEngine;

#endregion

// ReSharper disable once CheckNamespace
namespace Hono.Scripts.Battle.AbilityFramework
{
    public partial class AFunctionDefine
    {
        [AbilityFunction]
        public void DebugMessage(string flag, string msg, object p1, object p2, object p3)
        {
#if UNITY_EDITOR
            void Convert(string pattern, params object[] args)
            {
                Debug.Log(string.Format(pattern, args));
            }

            string flagStr = string.IsNullOrEmpty(flag) ? "" : $"[{flag}] ";

            Convert(flagStr + msg, p1, p2, p3);
#endif
        }

        #region 技能相关

        [AbilityFunction("Skill")]
        [AbilityFunctionDesc("修改Ability持有者的技能RT", null,
                             "技能id",
                             "修改技能等级/修改技能属性  /添加Tag /战斗资源消耗",
                             "技能等级   /属性类型     /tag     /战斗资源Id",
                             "-         /属性值      /-        /消耗修改数值"
        )]
        public void SkillRTModify(int skillId, ESkillModifyType modifyType, int p1, int p2)
        {
            if (!Unit.TryGetComponent<CombatComp>(out var combatComp))
            {
                return;
            }

            if (combatComp.TryGetSkillModifier(skillId, out SkillModifier modifier))
            {
                modifier.Modify(modifyType, p1, p2);
            }
        }

        [AbilityFunction("Skill")]
        [AbilityFunctionDesc("减少指定技能的Cd", null, "技能id", "减少值(秒)")]
        public void LessSkillCD(int skillId, float lessValue)
        {
            if (!Unit.TryGetComponent<CombatComp>(out var combatComp))
            {
                return;
            }

            if (combatComp.TryGetSkill(skillId, out var skill))
            {
                skill.LessCd(lessValue);
            }
        }

        [AbilityFunction("Skill")]
        [AbilityFunctionDesc("获取释放中的技能选中的世界坐标", null, "技能id")]
        public Vector3 GetSkillSelectWorldPos(int skillId)
        {
            if (!Unit.TryGetComponent<CombatComp>(out var combatComp))
            {
                return Vector3.zero;
            }

            if (combatComp.TryGetSkill(skillId, out var skill))
            {
                return skill.SelectWorldPos;
            }

            return Vector3.zero;
        }

        [AbilityFunction("Skill")]
        [AbilityFunctionDesc("获取释放中的技能选中的单体目标Uid", null, "技能id")]
        public int GetSkillSelectTargetUid(int skillId)
        {
            if (!Unit.TryGetComponent<CombatComp>(out var combatComp))
            {
                return -1;
            }

            if (combatComp.TryGetSkill(skillId, out var skill))
            {
                return skill.TargetUid;
            }

            return -1;
        }

        [AbilityFunction("Skill")]
        [AbilityFunctionDesc("获取释放中的技能选中的方向", "返回Y轴的旋转值", "技能id")]
        public float GetSkillSelectDir(int skillId)
        {
            if (!Unit.TryGetComponent<CombatComp>(out var combatComp))
            {
                return 0;
            }

            if (combatComp.TryGetSkill(skillId, out var skill))
            {
                return skill.SelectYAxisAngle;
            }

            return 0;
        }

        [AbilityFunction("Skill")]
        [AbilityFunctionDesc("获取释放中的Aoe技能选中的目标", "目标 Uid List", "技能id")]
        public List<int> GetSkillSelectTargets(int skillId)
        {
            if (!Unit.TryGetComponent<CombatComp>(out var combatComp))
            {
                return null;
            }

            if (combatComp.TryGetSkill(skillId, out var skill))
            {
                return skill.SelectUnitsInArea;
            }

            return null;
        }

        [AbilityFunction("Skill")]
        [AbilityFunctionDesc("添加战斗资源", null, "战斗资源Id", "数量")]
        public void AddFightRes(int resId, int value)
        {
            if (!Unit.TryGetComponent<CombatComp>(out var combatComp))
            {
                return;
            }

            combatComp.AddEnergy(resId, value);
        }

        #endregion

        #region Buff相关

        [AbilityFunction("Buff")]
        [AbilityFunctionDesc("获取buff层数", "返回buff层数", "buff持有者Uid", "BuffId", "buff来源Uid")]
        public int GetBuffLayer(int actorUid, int buffId, int buffSourceUid)
        {
            if (tryGetUnit(actorUid, out Unit actor))
            {
                if (actor.TryGetComponent<BuffComp>(out var comp))
                {
                    return comp.GetBuffLayer(buffId, buffSourceUid);
                }
            }

            return -1;
        }

        [AbilityFunction("Buff")]
        [AbilityFunctionDesc("添加buff", null, "Buff来源的Uid", "添加Buff的目标的Uid", "BuffId", "buff初始层数")]
        public void AddBuff(int sourceUid, int targetUid, int buffId, int buffLayer)
        {
            if (!tryGetUnit(sourceUid, out _))
            {
                return;
            }

            if (!tryGetUnit(targetUid, out var target))
            {
                return;
            }

            if (target.TryGetComponent(out BuffComp combatComp))
            {
                combatComp.AddBuff(buffId, sourceUid, buffLayer);
            }
        }

        [AbilityFunction("Buff")]
        [AbilityFunctionDesc("添加多个buff", null, "Buff来源的Uid", "添加Buff的目标Uid列表", "BuffId", "buff初始层数")]
        public void AddBuffToTargets(int buffSourceUid,
            List<int> targetUidList,
            int buffId,
            int buffLayer = 1)
        {
            if (targetUidList is not { Count: > 0 })
            {
                return;
            }

            if (!tryGetUnit(buffSourceUid, out _))
            {
                return;
            }


            foreach (var targetUid in targetUidList)
            {
                if (!tryGetUnit(targetUid, out var target))
                {
                    return;
                }

                var sourceUid = buffSourceUid;

                if (target.TryGetComponent(out BuffComp combatComp))
                {
                    combatComp.AddBuff(buffId, sourceUid, buffLayer);
                }
            }
        }

        [AbilityFunction("Buff")]
        [AbilityFunctionDesc("删除buff", null, "Buff持有者", "BuffId", "buff来源Uid")]
        public void RemoveBuff(int targetUid, int buffId, int buffSourceUid)
        {
            if (!tryGetUnit(targetUid, out var target))
            {
                return;
            }

            if (target.TryGetComponent(out BuffComp combatComp))
            {
                combatComp.RemoveBuff(buffId, buffSourceUid);
            }
        }

        #endregion

        #region Hit

        [AbilityFunction("Hit")]
        [AbilityFunctionDesc("单体Hit", null, "攻击者Uid", "受击者Uid", "伤害Id", "禁止触发事件")]
        public void SingleHit(int attackUid, int targetUid, int damageId, bool disableFireEvent)
        {
            if (!tryGetUnit(attackUid, out var attacker))
            {
                return;
            }

            if (!tryGetUnit(targetUid, out var target))
            {
                return;
            }

            HitSystem.Instance.SingleHit(attacker, target, getDamageSourceType(), AContext.Id, disableFireEvent,
                                         damageId);
        }

        [AbilityFunction("Hit")]
        [AbilityFunctionDesc("范围Hit", null, "攻击者Uid", "指定坐标", "旋转角度", "aoe相关设置", "伤害Id", "禁止触发事件")]
        public void AreaHit(int attackUid,
            Vector3 worldPos,
            float yAxisAngle,
            RangeFilterSetting aoeSetting,
            int damageId,
            bool disableFireEvent)
        {
            if (!tryGetUnit(attackUid, out var attacker))
            {
                return;
            }


            HitSystem.Instance.AreaHit(attacker, worldPos, yAxisAngle, getDamageSourceType(), AContext.Id, aoeSetting,
                                       disableFireEvent,
                                       damageId);
        }

        [AbilityFunction("Hit")]
        [AbilityFunctionDesc("脱手单体HitBox", null)]
        public void LockTargetSingleHitBox(int attackUid,
            int targetUid,
            int maxHitNumber,
            float delayTime,
            float interval,
            int damageId,
            bool disableFireEvent)
        {
            if (!tryGetUnit(attackUid, out var attacker))
            {
                return;
            }

            if (!tryGetUnit(targetUid, out var target))
            {
                return;
            }

            World.Current.CreateLockTargetSingleHitBox(attacker, target, getDamageSourceType(), AContext.Id,
                                                       maxHitNumber, delayTime, interval, disableFireEvent, damageId);
        }

        [AbilityFunction("Hit")]
        [AbilityFunctionDesc("脱手锁目标范围HitBox", null)]
        public void LockTargetAreaHitBox(int attackUid,
            int targetUid,
            int maxHitNumber,
            RangeFilterSetting aoeSetting,
            float delayTime,
            float interval,
            int damageId,
            bool disableFireEvent)
        {
            if (!tryGetUnit(attackUid, out var attacker))
            {
                return;
            }

            if (!tryGetUnit(targetUid, out var target))
            {
                return;
            }

            World.Current.CreateLockTargetAreaHitBox(attacker, target, getDamageSourceType(), AContext.Id,
                                                     maxHitNumber, aoeSetting, delayTime, interval, disableFireEvent,
                                                     damageId);
        }

        [AbilityFunction("Hit")]
        [AbilityFunctionDesc("脱手范围HitBox", null)]
        public void AreaHitBox(int attackUid,
            Vector3 worldPos,
            float yAxisAngle,
            int maxHitNumber,
            RangeFilterSetting aoeSetting,
            float delayTime,
            float interval,
            int damageId,
            bool disableFireEvent)
        {
            if (!tryGetUnit(attackUid, out var attacker))
            {
                return;
            }


            World.Current.CreateHitAreaBox(attacker, worldPos, yAxisAngle, getDamageSourceType(), AContext.Id,
                                           maxHitNumber, aoeSetting, delayTime, interval, disableFireEvent,
                                           damageId);
        }

        #endregion

        #region 子弹

        [AbilityFunction("Bullet")]
        public void CreateLockTargetBullet(
            int attackUid,
            int targetUid,
            int bulletId,
            Vector3 offset,
            float yAxisAngle,
            int hitTargetDamageId,
            int hitNotTargetDamageId = 0)
        {
            if (!tryGetUnit(attackUid, out var attacker))
            {
                return;
            }

            if (!tryGetUnit(targetUid, out var target))
            {
                return;
            }

            if (!AssetManager.Instance.TryGetData<BulletData>(bulletId, out var bulletData))
            {
                return;
            }

            var bullet = World.Current.CreateLockTargetBullet(attacker, target, bulletData, getDamageSourceType(), hitTargetDamageId,
                                                 hitNotTargetDamageId);
            bullet.UnitTransform.Pos += offset;
            bullet.UnitTransform.Rot = Quaternion.AngleAxis(yAxisAngle, Vector3.up);
        }

        [AbilityFunction("Bullet")]
        public void CreateDirectionBullet(
            int attackUid,
            float yAxisAngle,
            int bulletId,
            Vector3 offset,
            int hitTargetDamageId,
            int hitNotTargetDamageId = 0)
        {
            if (!tryGetUnit(attackUid, out var attacker))
            {
                return;
            }

            if (!AssetManager.Instance.TryGetData<BulletData>(bulletId, out var bulletData))
            {
                return;
            }

            var bullet = World.Current.CreateDirectionBullet(attacker, yAxisAngle, bulletData, getDamageSourceType(),
                                                hitTargetDamageId,
                                                hitNotTargetDamageId);
            bullet.UnitTransform.Pos += offset;
        }

        #endregion

        #region 特效

        [AbilityFunction("VFX")]
        public int AddVFXByKey(int targetUid, string key, VFXSetting setting)
        {
            if (!tryGetUnit(targetUid, out var target))
            {
                return -1;
            }

            if (target.TryGetComponent<VFXComp>(out var vfxComp))
            {
                return vfxComp.AddVFXByKey(key, setting);
            }

            Debug.LogError("创建VFX失败，目标没有特效组件");
            return -1;
        }

        [AbilityFunction("VFX")]
        public int AddVFXByPath(int targetUid, string path, VFXSetting setting)
        {
            if (!tryGetUnit(targetUid, out var target))
            {
                return -1;
            }

            if (target.TryGetComponent<VFXComp>(out var vfxComp))
            {
                return vfxComp.AddVFXByKey(path, setting);
            }

            Debug.LogError("创建VFX失败，目标没有特效组件");
            return -1;
        }


        [AbilityFunction("VFX")]
        public void RemoveVFX(int vfxTargetUid, int vfxUid)
        {
            if (!tryGetUnit(vfxTargetUid, out var target))
            {
                return;
            }

            if (target.TryGetComponent<VFXComp>(out var vfxComp))
            {
                vfxComp.RemoveVFX(vfxUid);
            }
        }

        #endregion

        #region 变量相关

        [AbilityFunction("Variable")]
        public int GetVariableBoardInt(bool isGlobal, string key)
        {
            if (isGlobal)
            {
                return AContext.Driver.VariableBoard.Get<int>(key);
            }

            if (AContext.VariableBoard.TryGet(key, out int result))
                return result;
            var rtBoard = AContext.GetRunNodeVariableBoard();
            return rtBoard?.Get<int>(key) ?? result;
        }

        [AbilityFunction("Variable")]
        public float GetVariableBoardFloat(bool isGlobal, string key)
        {
            if (isGlobal)
            {
                return AContext.Driver.VariableBoard.Get<float>(key);
            }

            if (AContext.VariableBoard.TryGet(key, out float result))
                return result;
            var rtBoard = AContext.GetRunNodeVariableBoard();
            return rtBoard?.Get<float>(key) ?? result;
        }

        [AbilityFunction("Variable")]
        public bool GetVariableBoardBool(bool isGlobal, string key)
        {
            if (isGlobal)
            {
                return AContext.Driver.VariableBoard.Get<bool>(key);
            }

            if (AContext.VariableBoard.TryGet(key, out bool result))
                return result;
            var rtBoard = AContext.GetRunNodeVariableBoard();
            return rtBoard?.Get<bool>(key) ?? result;
        }

        [AbilityFunction("Variable")]
        public Vector3 GetVariableBoardVec3(bool isGlobal, string key)
        {
            if (isGlobal)
            {
                return AContext.Driver.VariableBoard.Get<Vector3>(key);
            }

            if (AContext.VariableBoard.TryGet(key, out Vector3 result))
                return result;
            var rtBoard = AContext.GetRunNodeVariableBoard();
            return rtBoard?.Get<Vector3>(key) ?? result;
        }

        [AbilityFunction("Variable")]
        public object GetVariableBoardObject(bool isGlobal, string key)
        {
            if (isGlobal)
            {
                return AContext.Driver.VariableBoard.GetRef(key);
            }

            if (AContext.VariableBoard.TryGetRef(key, out object result))
                return result;
            var rtBoard = AContext.GetRunNodeVariableBoard();
            return rtBoard?.GetRef(key) ?? result;
        }

        #endregion

        [AbilityFunction]
        public float GetUnitYAxisAngle(int unitUid)
        {
            if (tryGetUnit(unitUid, out var unit))
            {
                return unit.UnitTransform.YAxisAngle;
            }

            return 0;
        }

        [AbilityFunction]
        public Vector3 GetUnitPosition(int unitUid)
        {
            if (tryGetUnit(unitUid, out var unit))
            {
                return unit.UnitTransform.Pos;
            }

            return Vector3.zero;
        }

        [AbilityFunction]
        public List<int> SelectTargets(int useFilterUnitUid, Vector3 pos, float yAxisAngle, RangeFilterSetting setting)
        {
            if (tryGetUnit(useFilterUnitUid, out var actor))
            {
                World.Query.SearchUnits(actor, pos, yAxisAngle, setting, ref _abilitySelectTargetUids);
            }

            return _abilitySelectTargetUids;
        }

        [AbilityFunction]
        public bool CheckTalent(int targetUid, string talentKey)
        {
            return true;
        }

        [AbilityFunction]
        public bool CheckTag(int targetUid, int tag, bool reverse)
        {
            if (tryGetUnit(targetUid, out var actor))
            {
                return reverse ? !actor.Tags.HasTag(tag) : actor.Tags.HasTag(tag);
            }

            return false;
        }


        [AbilityFunction]
        public int GetListCount(List<int> list)
        {
            if (list == null)
            {
                Debug.LogWarning("参数为空！");
                return 0;
            }

            return list.Count;
        }

        [AbilityFunction]
        public int GetIntListItem(List<int> list, int index)
        {
            if (list == null)
            {
                Debug.LogError("列表为空！");
                return 0;
            }

            if (index >= list.Count)
            {
                Debug.LogError($"  {AContext.Id}  索引长度超过列表长度！");
                return 0;
            }

            return list[index];
        }

        [AbilityFunction]
        public List<int> GetListRange(List<int> list, int right)
        {
            if (list == null)
            {
                Debug.LogWarning("参数为空！");
            }

            Debug.LogWarning(list.Count.ToString());
            return list.GetRange(0, Mathf.Min(list.Count, right));
        }

        [AbilityFunction]
        public int CalculateInt(int left, ECalculateType calculateType, int right)
        {
            switch (calculateType)
            {
                case ECalculateType.Add:
                    return left + right;
                case ECalculateType.Subtract:
                    return left - right;
                case ECalculateType.Multiply:
                    return left * right;
                case ECalculateType.Divide:
                    return right == 0 ? 0 : left / right;
            }

            return 0;
        }


        [AbilityFunction]
        public float CalculateFloat(float left, ECalculateType calculateType, float right)
        {
            switch (calculateType)
            {
                case ECalculateType.Add:
                    return left + right;
                case ECalculateType.Subtract:
                    return left - right;
                case ECalculateType.Multiply:
                    return left * right;
                case ECalculateType.Divide:
                    return right == 0 ? 0 : left / right;
            }

            return 0;
        }

        [AbilityFunction]
        public bool And(bool a, bool b) => a && b;

        [AbilityFunction]
        public bool Or(bool a, bool b) => a || b;

        [AbilityFunction]
        public bool CompareInt(int left, ECompareResType compareType, int right)
        {
            int res = left.CompareTo(right);
            return CommonUtility.GetCompareResult(compareType, res);
        }

        [AbilityFunction]
        public bool CompareFloat(float left, ECompareResType compareType, float right)
        {
            int res = left.CompareTo(right);
            return CommonUtility.GetCompareResult(compareType, res);
        }

        #region 数学函数

        [AbilityFunction]
        public int IntSelfAdditive(int self) => ++self;

        [AbilityFunction]
        public float FloatSelfAdditive(float self) => ++self;

        [AbilityFunction]
        public int IntSelfSubtracting(int self) => --self;

        [AbilityFunction]
        public float FloatSelfSubtracting(float self) => --self;

        #endregion
    }
}