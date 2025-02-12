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
        public int GetBuffLayer(int actorUid, int buffId)
        {
            if (tryGetActor(actorUid, out Actor actor))
            {
                if (actor.TryGetComponent<CombatComp>(out var comp))
                {
                    return comp.GetBuffLayer(buffId);
                }
            }

            return -1;
        }

        [AbilityFunction]
        [AbilityFunctionDesc("改技能等级", null, "技能id", "技能等级")]
        public void ChangeSkillLevel(int skillId, int level) { }

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

        [AbilityFunction("HitBox")]
        public void CreateHitBox(int attackUid, int targetUid, HitParams aoeHitSetting, bool fromTopSummer = false)
        {
            if (!tryGetActor(attackUid, out var attack))
            {
                return;
            }

            if (!tryGetActor(targetUid, out var target))
            {
                return;
            }
        }

        [AbilityFunction("HitBox")]
        public void CreateHitBoxes(int attackUid,
            List<int> targetUids,
            HitParams aoeHitSetting,
            bool fromTopSummer = false)
        {
            //返回打击点的Uid
            if (targetUids is not { Count: > 0 }) return;

            if (!tryGetActor(attackUid, out var attack))
            {
                return;
            }

            //返回打击点的Uid
            if (targetUids is not { Count: > 0 }) return;

            foreach (var targetUid in targetUids)
            {
                if (targetUid == 0) continue;
            }
        }

        [AbilityFunction("HitBox")]
        [AbilityFunctionDesc("创建打击盒子攻击指定目标", "无返回值", "打击盒子信息")]
        public void CreateHitBoxToTargets(HitParams aoeHitSetting)
        {
            //返回打击点的Uid
            var targetUids = new List<int>();
            if (targetUids == null)
            {
                Debug.LogWarning($"form abilityId {AContext.Id}目标列表是空的，未创建打击点！");
                return;
            }

            foreach (var targetUid in targetUids) { }
        }

        [AbilityFunction("Bullet")]
        public void CreateBullet(int targetUid, int bulletId, bool fromTopSummer = false) { }

        [AbilityFunction("Bullet")]
        public void CreateBullets(List<int> targetUids, int bulletId, bool fromTopSummer = false)
        {
            if (targetUids is not { Count: > 0 }) return;

            foreach (var targetUid in targetUids) { }
        }

        [AbilityFunction("Bullet")]
        public int AddVFX(VFXSetting setting, int vfxTargetUid = 0)
        {
            if (!tryGetActor(vfxTargetUid, out var target))
            {
                return -1;
            }

            if (target.TryGetComponent<VFXComp>(out var vfxComp))
            {
                // return vfxComp.AddVFXObject(setting);
            }

            Debug.LogError("创建VFX失败，目标没有特效组件");
            return -1;
        }

        [AbilityFunction("VfX")]
        public void AddVFXToTargets(VFXSetting setting, List<int> vfxTargetUids)
        {
            if (vfxTargetUids is not { Count: > 0 })
            {
                return;
            }

            foreach (var actorUid in vfxTargetUids)
            {
                if (tryGetActor(actorUid, out var target))
                {
                    if (target.TryGetComponent<VFXComp>(out var vfxComp))
                    {
                        // vfxComp.AddVFXObject(setting);
                    }
                }
            }

            return;
        }

        [AbilityFunction("VfX")]
        public void RemoveVFX(int vfxUid, int vfxTargetUid = 0)
        {
            if (!tryGetActor(vfxTargetUid, out var target))
            {
                return;
            }

            if (target.TryGetComponent<VFXComp>(out var vfxComp))
            {
                vfxComp.RemoveVFX(vfxUid);
            }
        }


        [AbilityFunction]
        public List<int> SelectTargets(int centerActorUid, RangeFilterSetting setting)
        {
            List<int> actorUids = new();
            if (tryGetActor(centerActorUid, out var actor))
            {
                //UnitManager.Instance.UseFilter(actor, setting, ref actorUids);
            }

            return actorUids;
        }

        [AbilityFunction]
        public bool CheckTalent(string talentKey)
        {
            return true;
        }

        [AbilityFunction]
        public bool CheckActorTag(int uid, int tagID, bool reverse)
        {
            /*if (tryGetActor(uid, out var actor)) {
                return reverse ? !actor.TagCollection.HasTag(tagID) : actor.TagCollection.HasTag(tagID);
            }
            return false;*/
            return false;
        }

        [AbilityFunction]
        public void AddBuff(int targetUid, int buffId, int buffLayer = 1, bool topSourceActor = false)
        {
            if (!tryGetActor(targetUid, out var target))
            {
                return;
            }

            var sourceActor = Actor;
            var sourceUid = topSourceActor
                ? sourceActor.GetAttr(EAttrType.AttrSourceActorUid)
                : sourceActor.GetAttr(EAttrType.AttrTopSourceActorUid);

            if (target.TryGetComponent(out CombatComp combatComp))
            {
                combatComp.AddBuff(buffId, sourceUid, buffLayer);
            }
        }

        [AbilityFunction]
        public void AddBuffToTargets(List<int> targetUids,
            int buffId,
            int buffLayer = 1,
            bool topSourceActor = false)
        {
            if (targetUids is not { Count: > 0 })
            {
                return;
            }

            foreach (var targetUid in targetUids)
            {
                if (!tryGetActor(targetUid, out var target))
                {
                    return;
                }

                var sourceActor = Actor;
                var sourceUid = topSourceActor
                    ? sourceActor.GetAttr(EAttrType.AttrSourceActorUid)
                    : sourceActor.GetAttr(EAttrType.AttrTopSourceActorUid);

                if (target.TryGetComponent(out CombatComp combatComp))
                {
                    combatComp.AddBuff(buffId, sourceUid, buffLayer);
                }
            }
        }

        [AbilityFunction]
        public void RemoveBuff(int targetUid, int buffId, int buffSourceUid)
        {
            if (!tryGetActor(targetUid, out var target))
            {
                return;
            }

            if (target.TryGetComponent(out CombatComp combatComp))
            {
                combatComp.RemoveBuff(buffId, buffSourceUid);
            }
        }

        [AbilityFunction]
        public int GetBuffSoruce(int buffId)
        {
            return -1; //BuffSystem.Instance.GetBuffSource(Actor.Uid, buffId);;
        }

        [AbilityFunction]
        public void LessSkillCD(int skillId, int lessValue) { }

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
        public void AddBattleResource(int resourceValue, bool isPer)
        {
            //resourceValue -> 万分比
            var maxMp = Actor.GetAttr(EAttrType.AttrMaxMp);
            var curMp = Actor.GetAttr(EAttrType.AttrMp);
            var value = resourceValue;
            if (isPer)
            {
                curMp = (int)(curMp * ((10000f + value) / 10000f));
            }
            else
            {
                curMp += (int)value;
            }

            curMp = Mathf.Clamp(curMp, 0, maxMp);
            Actor.SetAttr(EAttrType.AttrMp, curMp, false);
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