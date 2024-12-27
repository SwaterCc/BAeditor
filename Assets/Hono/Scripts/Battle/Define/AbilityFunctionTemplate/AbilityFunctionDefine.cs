#region

using System.Collections.Generic;
using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Tools;
using Hono.Scripts.Battle.Tools.CustomAttribute;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    /// <summary>
    ///     编辑器默认显示函数
    ///     不允许重载函数
    ///     尽量不要在get函数里直接获取对象
    ///     不要随便改函数名
    /// </summary>
    public partial class AbilityFunctionDefine
    {
        [AbilityFunction]
        public static int GetBuffLayer(int actorUid, int buffId)
        {
            if (ARunningTime.TryGetActor(actorUid, out Actor actor))
            {
                if (actor.Logic.TryGetComponent<ActorLogic.BuffComp>(out var comp))
                {
                    return comp.GetBuffLayer(buffId);
                }
            }

            return -1;
        }

        [AbilityFunction]
        public static int GetSkillLevel(int skillId)
        {
            return ARunningTime.Actor.Logic.GetSkillLevel(skillId);
        }

        [AbilityFunction]
        [AbilityFunctionDesc("改技能等级",null,"技能id","技能等级")]
        public static void ChangeSkillLevel(int skillId, int level)
        {
            ARunningTime.Actor.Logic.ChangeSkillLevel(skillId, level);
        }

        [AbilityFunction]
        public static void DebugMessage(string flag, string msg, object p1, object p2, object p3)
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
        public static void CreateHitBox(int attackUid, int targetUid, HitBoxData hitData, bool fromTopSummer = false)
        {
            if (!ARunningTime.TryGetActor(attackUid, out var attack))
            {
                return;
            }

            if (!ARunningTime.TryGetActor(targetUid, out var target))
            {
                return;
            }

            ActorManager.Instance.SummonActor(attack, EActorType.HitBox,
                1, fromTopSummer,
                (hitBox) =>
                {
                    hitBox.SetAttr(EAttrType.AttrSourceAbilityId, ARunningTime.AContext.Id, false);
                    hitBox.Variables.Set("hitBoxData", hitData);
                    //hitBox.Variables.Set("targetUid", targetId);
                    hitBox.Variables.Set("abilityTags", ARunningTime.AContext.TagCollection.GetAllTag());
                }
            );
        }

        [AbilityFunction("HitBox")]
        public static void CreateHitBoxes(int attackUid, List<int> targetUids, HitBoxData hitData,
            bool fromTopSummer = false)
        {
            //返回打击点的Uid
            if (targetUids is not { Count: > 0 }) return;

            if (!ARunningTime.TryGetActor(attackUid, out var attack))
            {
                return;
            }

            //返回打击点的Uid
            if (targetUids is not { Count: > 0 }) return;

            foreach (var targetUid in targetUids)
            {
                if (targetUid == 0) continue;

                ActorManager.Instance.SummonActor(attack, EActorType.HitBox,
                    1, fromTopSummer, (hitBox) =>
                    {
                        hitBox.SetAttr(EAttrType.AttrSourceAbilityId, ARunningTime.AContext.Id, false);
                        hitBox.Variables.Set("hitBoxData", hitData);
                        //hitBox.Variables.Set("targetUid", targetUid);
                        hitBox.Variables.Set("abilityTags", ARunningTime.AContext.TagCollection.GetAllTag());
                    });
            }
        }
        
        [AbilityFunction("HitBox")]
        [AbilityFunctionDesc("创建打击盒子攻击指定目标","无返回值","打击盒子信息")]
        public static void CreateHitBoxToTargets(HitBoxData hitData)
        {
            //返回打击点的Uid
            var targetUids = new List<int>();
            if (targetUids == null)
            {
                Debug.LogWarning($"form abilityId {ARunningTime.AContext.Id}目标列表是空的，未创建打击点！");
                return;
            }

            foreach (var targetUid in targetUids)
            {
                if (!ActorManager.Instance.TryGetActor(targetUid, out var target))
                {
                    continue;
                }

                ActorManager.Instance.SummonActor(ARunningTime.Actor, EActorType.HitBox,
                    1, false, (hitBox) =>
                    {
                        hitBox.SetAttr(EAttrType.AttrSourceAbilityId, ARunningTime.AContext.Id, false);
                        hitBox.Variables.Set("hitBoxData", hitData);
                        //hitBox.Variables.Set("targetUid", targetUid);
                        //hitBox.Variables.Set("targetPos", target.Pos);
                        hitBox.Variables.Set("abilityTags", ARunningTime.AContext.TagCollection.GetAllTag());
                    });
            }
        }

        [AbilityFunction("Bullet")]
        public static void CreateBullet(int targetUid, int bulletId, bool fromTopSummer = false)
        {
            var bullet = ActorManager.Instance.SummonActor(ARunningTime.Actor, EActorType.Bullet,
                bulletId, fromTopSummer, (bullet) =>
                {
                    bullet.SetAttr(EAttrType.AttrSourceAbilityId, ARunningTime.AContext.Id, false);
                    //bullet.Variables.Set("targetUid", targetUid);
                    bullet.Variables.Set("abilityTags", ARunningTime.AContext.TagCollection.GetAllTag());
                });
        }

        [AbilityFunction("Bullet")]
        public static void CreateBullets(List<int> targetUids, int bulletId, bool fromTopSummer = false)
        {
            if (targetUids is not { Count: > 0 }) return;

            foreach (var targetUid in targetUids)
            {
                var bullet = ActorManager.Instance.SummonActor(ARunningTime.Actor, EActorType.Bullet,
                    bulletId, fromTopSummer, (bullet) =>
                    {
                        bullet.SetAttr(EAttrType.AttrSourceAbilityId, ARunningTime.AContext.Id, false);
                        //bullet.Variables.Set("targetUid", targetUid);
                        bullet.Variables.Set("abilityTags", ARunningTime.AContext.TagCollection.GetAllTag());
                    });
            }
        }

        [AbilityFunction("Bullet")]
        public static int AddVFX(VFXSetting setting, int vfxTargetUid = 0)
        {
            if (!ARunningTime.TryGetActor(vfxTargetUid, out var target))
            {
                return -1;
            }

            if (target.Logic.TryGetComponent<ActorLogic.VFXComp>(out var vfxComp))
            {
                return vfxComp.AddVFXObject(setting);
            }

            Debug.LogError("创建VFX失败，目标没有特效组件");
            return -1;
        }

        [AbilityFunction("VfX")]
        public static void AddVFXToTargets(VFXSetting setting, List<int> vfxTargetUids)
        {
            if (vfxTargetUids is not { Count: > 0 })
            {
                return;
            }

            foreach (var actorUid in vfxTargetUids)
            {
                if (ARunningTime.TryGetActor(actorUid, out var target))
                {
                    if (target.Logic.TryGetComponent<ActorLogic.VFXComp>(out var vfxComp))
                    {
                        vfxComp.AddVFXObject(setting);
                    }
                }
            }

            return;
        }

        [AbilityFunction("VfX")]
        public static void RemoveVFX(int vfxUid, int vfxTargetUid = 0)
        {
            if (!ARunningTime.TryGetActor(vfxTargetUid, out var target))
            {
                return;
            }

            if (target.Logic.TryGetComponent<ActorLogic.VFXComp>(out var vfxComp))
            {
                vfxComp.RemoveVFX(vfxUid);
            }
        }

        [AbilityFunction]
        public static int AddMotion(int moveTargetUid, MotionSetting motionSetting)
        {
            if (ARunningTime.Actor.Logic.TryGetComponent<ActorLogic.MotionComp>(out var motionComp))
            {
                return motionComp.AddMotion(moveTargetUid, motionSetting);
            }

            return -1;
        }

        [AbilityFunction]
        public static void RemoveMotion(int motionUid)
        {
            if (ARunningTime.Actor.Logic.TryGetComponent<ActorLogic.MotionComp>(out var motionComp))
            {
                motionComp.RemoveMotion(motionUid);
            }
        }

        [AbilityFunction]
        public static void SendMsg(int actorUid, string msg, object p1, object p2, object p3, object p4, object p5)
        {
            if (ARunningTime.TryGetActor(actorUid, out var actor))
            {
                var msgCache = new MsgCache()
                {
                    MsgKey = msg,
                    P1 = p1,
                    P2 = p2,
                    P3 = p3,
                    P4 = p4,
                    P5 = p5,
                };
                MessageCenter.Instance.AddMsg(actor.Uid, msgCache);
            }
        }

        [AbilityFunction]
        public static List<int> SelectTargets(int centerActorUid, RangeFilterSetting setting)
        {
            List<int> actorUids = new();
            if (ARunningTime.TryGetActor(centerActorUid, out var actor))
            {
                ActorManager.Instance.UseFilter(actor, setting, ref actorUids);
            }

            return actorUids;
        }

        [AbilityFunction]
        public static void ResetActorTargets(int centerActorUid, RangeFilterSetting setting)
        {
            /*List<int> actorUids = new();
            if (ARunningTime.TryGetActor(centerActorUid, out var actor)) {
                ActorManager.Instance.UseFilter(actor, setting, ref actorUids);
                ARunningTime.Actor.SetAttr(EAttrType.AttrAttackTargetUids, actorUids, false);
            }*/
        }

        [AbilityFunction]
        public static bool CheckTalent(string talentKey)
        {
            return true;
        }

        [AbilityFunction]
        public static bool CheckActorTag(int uid, int tagID, bool reverse)
        {
            /*if (ARunningTime.TryGetActor(uid, out var actor)) {
                return reverse ? !actor.TagCollection.HasTag(tagID) : actor.TagCollection.HasTag(tagID);
            }
            return false;*/
            return false;
        }

        [AbilityFunction]
        public static void AddBuff(int targetUid, int buffId, int buffLayer = 1, bool topSourceActor = false)
        {
            if (!ARunningTime.TryGetActor(targetUid, out var target))
            {
                return;
            }

            if (target.Logic.TryGetComponent<ActorLogic.BuffComp>(out var comp))
            {
                var sourceActor = ARunningTime.Actor;
                var sourceUid = topSourceActor
                    ? sourceActor.GetAttr(EAttrType.AttrSourceActorUid)
                    : sourceActor.GetAttr(EAttrType.AttrTopSourceActorUid);
                comp.AddBuff(sourceUid, buffId, buffLayer);
            }
        }

        [AbilityFunction]
        public static void AddBuffToTargets(List<int> targetUids, int buffId, int buffLayer = 1,
            bool topSourceActor = false)
        {
            if (targetUids is not { Count: > 0 })
            {
                return;
            }

            foreach (var actorUid in targetUids)
            {
                if (!ARunningTime.TryGetActor(actorUid, out var actor))
                {
                    return;
                }

                if (actor.Logic.TryGetComponent<ActorLogic.BuffComp>(out var comp))
                {
                    var sourceActor = ARunningTime.Actor;
                    var sourceUid = topSourceActor
                        ? sourceActor.GetAttr(EAttrType.AttrSourceActorUid)
                        : sourceActor.GetAttr(EAttrType.AttrTopSourceActorUid);
                    comp.AddBuff(sourceUid, buffId, buffLayer);
                }
            }
        }

        [AbilityFunction]
        public static void RemoveBuff(int buffOwnerActorUid, int buffId, int buffLayer = 1)
        {
            if (!ARunningTime.TryGetActor(buffOwnerActorUid, out var actor))
            {
                return;
            }

            if (actor.Logic.TryGetComponent<ActorLogic.BuffComp>(out var comp))
            {
                comp.RemoveBuff(buffId);
            }
        }

        [AbilityFunction]
        public static void RemoveTargetsBuff(List<int> targetUids, int buffId, int buffLayer = 1)
        {
            if (targetUids is not { Count: > 0 })
            {
                return;
            }

            foreach (var actorUid in targetUids)
            {
                if (!ARunningTime.TryGetActor(actorUid, out var actor))
                {
                    return;
                }

                if (actor.Logic.TryGetComponent<ActorLogic.BuffComp>(out var comp))
                {
                    comp.RemoveBuff(buffId);
                }
            }
        }

        [AbilityFunction]
        public static int GetBuffSoruce(int buffId)
        {
            if (ARunningTime.Actor.Logic.TryGetComponent<ActorLogic.BuffComp>(out var comp))
            {
                return comp.GetBuffSource(buffId);
            }

            return -1;
        }

        [AbilityFunction]
        public static void LessSkillCD(int skillId, int lessValue)
        {
            if (!ARunningTime.TryGetActor(0, out var actor))
            {
                return;
            }

            if (actor.Logic.TryGetComponent<ActorLogic.SkillComp>(out var comp))
            {
                if (comp.TryGetSkill(skillId, out var skill))
                {
                    skill.LessCd(lessValue / 10000f);
                }
            }
        }

        [AbilityFunction]
        public static int GetListCount(List<int> list)
        {
            if (list == null)
            {
                Debug.LogWarning("参数为空！");
                return 0;
            }

            return list.Count;
        }

        [AbilityFunction]
        public static int GetIntListItem(List<int> list, int index)
        {
            if (list == null)
            {
                Debug.LogError("列表为空！");
                return 0;
            }

            if (index >= list.Count)
            {
                Debug.LogError($"  {ARunningTime.AContext.Id}  索引长度超过列表长度！");
                return 0;
            }

            return list[index];
        }

        [AbilityFunction]
        public static List<int> GetListRange(List<int> list, int right)
        {
            if (list == null)
            {
                Debug.LogWarning("参数为空！");
            }

            Debug.LogWarning(list.Count.ToString());
            return list.GetRange(0, Mathf.Min(list.Count, right));
        }

        [AbilityFunction]
        public static int CalculateInt(int left, ECalculateType calculateType, int right)
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
        public static void AddBattleResource(int resourceValue, bool isPer)
        {
            //resourceValue -> 万分比
            var maxMp = ARunningTime.Actor.GetAttr(EAttrType.AttrMaxMp);
            var curMp = ARunningTime.Actor.GetAttr(EAttrType.AttrMp);
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
            ARunningTime.Actor.SetAttr(EAttrType.AttrMp, curMp, false);
        }

        [AbilityFunction]
        public static float CalculateFloat(float left, ECalculateType calculateType, float right)
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
        public static bool And(bool a, bool b) => a && b;

        [AbilityFunction]
        public static bool Or(bool a, bool b) => a || b;

        [AbilityFunction]
        public static bool CompareInt(int left, ECompareResType compareType, int right)
        {
            int res = left.CompareTo(right);
            return CommonUtility.GetCompareResult(compareType, res);
        }

        [AbilityFunction]
        public static bool CompareFloat(float left, ECompareResType compareType, float right)
        {
            int res = left.CompareTo(right);
            return CommonUtility.GetCompareResult(compareType, res);
        }

        #region 数学函数

        [AbilityFunction]
        public static int IntSelfAdditive(int self) => ++self;

        [AbilityFunction]
        public static float FloatSelfAdditive(float self) => ++self;

        [AbilityFunction]
        public static int IntSelfSubtracting(int self) => --self;

        [AbilityFunction]
        public static float FloatSelfSubtracting(float self) => --self;

        #endregion


        #region 场景流程

        [AbilityFunction]
        public static void ChangeSoliderType(int soliderId)
        {
            if (ARunningTime.Actor.ActorType != EActorType.Building)
            {
                return;
            }

            if (ARunningTime.Actor.Logic is not BarrackLogic barrackLogic)
            {
                return;
            }

            //barrackLogic.ChangeSoliderId(soliderId);
        }

        [AbilityFunction]
        public static void SetLootList(LootList lootList)
        {
            BattleManager.CurBattle.LootController.SetLootList(ref lootList);
        }

        [AbilityFunction]
        public static void ChangeLootRule(ELootDropRule dropRule, int killNumOrTag)
        {
            BattleManager.CurBattle.LootController.SwitchRule(dropRule, killNumOrTag);
        }

        [AbilityFunction]
        public static void CreateLoot(RefVector3 lootPos)
        {
            BattleManager.CurBattle.LootController.CreateLoot(lootPos);
        }

        [AbilityFunction]
        public static int CurRoundCount()
        {
            return BattleManager.CurBattle.RtInfo.CurRoundCount;
        }

        [AbilityFunction]
        public static void CallMonsterGeneratorRun(int uid, int configId)
        {
            var monsterGenEventInfo = new MonsterGenEventInfo
            {
                MonsterConfigId = configId, SingleUid = uid, Behave = EMonsterGenBehave.Summon
            };
            BattleEventManager.Instance.TriggerActorEvent(uid, EBattleEventType.OnCallMonsterGenerator,
                monsterGenEventInfo);
        }

        #endregion
    }
}