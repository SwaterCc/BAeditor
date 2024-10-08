using System.Collections.Generic;
using Hono.Scripts.Battle.Tools.CustomAttribute;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public static partial class AbilityFunction
    {
        /// <summary>
        /// 编辑器默认显示函数
        /// 不允许重载函数
        /// 尽量不要在get函数里直接获取对象
        /// 不要随便改函数名
        /// </summary>
        [AbilityMethod]
        public static void SetNextStageId(int id)
        {
            Ability.Context.Invoker.SetNextGroupId(id);
        }

        [AbilityMethod]
        public static void StopStage()
        {
            Ability.Context.Invoker.StopGroup();
        }

        [AbilityMethod]
        public static int GetBuffLayer(int actorUid, int buffConfigId)
        {
            if (tryGetActor(actorUid, out var actor))
            {
                if (actor.Logic.TryGetComponent<ActorLogic.BuffComp>(out var comp))
                {
                    return comp.GetBuffLayer(buffConfigId);
                }
            }

            return -1;
        }

        [AbilityMethod]
        public static void ClearVariables()
        {
            Ability.Context.Invoker.Variables.Clear();
        }

        [AbilityMethod]
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

        [AbilityMethod]
        public static void CreateHitBox(int attackUid, int targetUid, HitBoxData hitData, bool fromTopSummer = false)
        {
			if (!tryGetActor(attackUid, out var attack)) {
				return;
			}

			if (!tryGetActor(targetUid, out var target)) {
				return;
			}

			var hitBox = ActorManager.Instance.SummonActor(attack, EActorType.HitBox,
                ActorManager.NormalHitBoxConfigId, fromTopSummer);
			hitBox.SetAttr<int>(ELogicAttr.AttrSourceAbilityConfigId, Ability.Context.Invoker.ConfigId, false);
            hitBox.Variables.Set("hitBoxData", hitData);
            hitBox.Variables.Set("targetUid", target.Uid);
            hitBox.Variables.Set("abilityTags", Ability.Context.Invoker.Tags.GetAllTag());
            ActorManager.Instance.AddActor(hitBox);
        }

        [AbilityMethod]
        public static void CreateHitBoxes(int attackUid,List<int> targetUids, HitBoxData hitData, bool fromTopSummer = false)
        {

			if (!tryGetActor(attackUid, out var attack)) {
				return;
			}

			//返回打击点的Uid
			if (targetUids is not { Count: > 0 }) return;

            foreach (var targetUid in targetUids)
            {
                if (targetUid == 0) continue;

				var hitBox = ActorManager.Instance.SummonActor(attack, EActorType.HitBox,
				ActorManager.NormalHitBoxConfigId, fromTopSummer);
				hitBox.SetAttr<int>(ELogicAttr.AttrSourceAbilityConfigId, Ability.Context.Invoker.ConfigId, false);
				hitBox.Variables.Set("hitBoxData", hitData);
				hitBox.Variables.Set("targetUid", targetUid);
				hitBox.Variables.Set("abilityTags", Ability.Context.Invoker.Tags.GetAllTag());
				ActorManager.Instance.AddActor(hitBox);
			}
        }

        [AbilityMethod]
        public static void CreateHitBoxToTargets(HitBoxData hitData)
        {
            //返回打击点的Uid
            var targetUids = Ability.Context.SourceActor.GetAttr<List<int>>(ELogicAttr.AttrAttackTargetUids);
            if (targetUids == null)
            {
                Debug.LogWarning($"form abilityId {Ability.Context.Invoker.Uid}目标列表是空的，未创建打击点！");
                return;
            }

            foreach (var targetUid in targetUids)
            {
                if (targetUid == 0) continue;

                var hitBox = ActorManager.Instance.SummonActorByAbility(Ability.Context.Invoker, EActorType.HitBox,
                    ActorManager.NormalHitBoxConfigId, false);
                hitBox.Variables.Set("hitBoxData", hitData);
                hitBox.Variables.Set("targetUid", targetUid);
                hitBox.Variables.Set("abilityTags", Ability.Context.Invoker.Tags.GetAllTag());
                ActorManager.Instance.AddActor(hitBox);
            }
        }

        [AbilityMethod]
        public static void CreateBullet(int targetUid, int bulletConfigId, bool fromTopSummer = false)
        {
            var bullet = ActorManager.Instance.SummonActorByAbility(Ability.Context.Invoker,EActorType.Bullet,bulletConfigId,fromTopSummer);
            bullet.Variables.Set("targetUid", targetUid);
            bullet.Variables.Set("abilityTags", Ability.Context.Invoker.Tags.GetAllTag());
            ActorManager.Instance.AddActor(bullet);
        }

        [AbilityMethod]
        public static void CreateBullets(List<int> targetUids, int bulletConfigId, bool fromTopSummer = false)
        {
            if (targetUids is not { Count: > 0 }) return;
            
            foreach (var targetUid in targetUids)
            {
                var bullet = ActorManager.Instance.SummonActorByAbility(Ability.Context.Invoker,EActorType.Bullet,bulletConfigId,fromTopSummer);
                bullet.Variables.Set("targetUid", targetUid);
                bullet.Variables.Set("abilityTags", Ability.Context.Invoker.Tags.GetAllTag());
                ActorManager.Instance.AddActor(bullet);
            }
        }

        [AbilityMethod]
        public static int AddVFX(VFXSetting setting,int vfxTargetUid = 0)
        {
			if(!tryGetActor(vfxTargetUid,out var target)) {
				return -1;
			}

            if (target.Logic.TryGetComponent<ActorLogic.VFXComp>(out var vfxComp))
            {
                return vfxComp.AddVFXObject(setting);
            }

            Debug.LogError("创建VFX失败，目标没有特效组件");
            return -1;
        }

        [AbilityMethod]
        public static void RemoveVFX(int vfxUid, int vfxTargetUid = 0)
        {
			if (!tryGetActor(vfxTargetUid, out var target)) {
				return;
			}
			if (target.Logic.TryGetComponent<ActorLogic.VFXComp>(out var vfxComp))
            {
                vfxComp.RemoveVFX(vfxUid);
            }
        }

        [AbilityMethod]
        public static int AddMotion(int moveTargetUid, MotionSetting motionSetting)
        {
            if (Ability.Context.SourceActor.Logic.TryGetComponent<ActorLogic.MotionComp>(out var motionComp))
            {
                return motionComp.AddMotion(moveTargetUid, motionSetting);
            }

            return -1;
        }

        [AbilityMethod]
        public static void RemoveMotion(int motionUid)
        {
            if (Ability.Context.SourceActor.Logic.TryGetComponent<ActorLogic.MotionComp>(out var motionComp))
            {
                motionComp.RemoveMotion(motionUid);
            }
        }

        [AbilityMethod]
        public static void SendMsg(int actorUid, string msg, object p1, object p2, object p3, object p4, object p5)
        {
            if (tryGetActor(actorUid, out var actor))
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

        [AbilityMethod]
        public static List<int> SelectTargets(int centerActorUid, FilterSetting setting)
        {
            List<int> actorUids = new();
            if (tryGetActor(centerActorUid, out var actor))
            {
                actorUids = ActorManager.Instance.UseFilter(actor, setting);
            }

            return actorUids;
        }

        [AbilityMethod]
        public static void ResetActorTargets(int centerActorUid, FilterSetting setting)
        {
            if (tryGetActor(centerActorUid, out var actor))
            {
                var actorUids = ActorManager.Instance.UseFilter(actor, setting);
                Ability.Context.SourceActor.SetAttr(ELogicAttr.AttrAttackTargetUids, actorUids, false);
            }
        }

        [AbilityMethod]
        public static bool CheckTalent(string talentKey)
        {
            return true;
        }

        [AbilityMethod]
        public static bool CheckAbilityTag(int abilityConfigId, int tagID)
        {
            if (Ability.Context.SourceActor.TryGetAbility(abilityConfigId, out var ability))
            {
                return ability.Tags.HasTag(tagID);
            }

            return false;
        }

        [AbilityMethod]
        public static float MakeDamageBySource(int attackUid, int targetUid, int damageConfigId, int skillRate,
            bool isCriticalOnce)
        {
            if (!tryGetActor(targetUid, out var target))
            {
                return -1;
            }

            if (!tryGetActor(attackUid, out var attack))
            {
                return -1;
            }

            var damageConfig = new DamageConfig();
            var damage = ConfigManager.Table<DamageTable>().Get(damageConfigId);
            damageConfig.DamageType = (EDamageType)damage.DamageType;
            damageConfig.ElementType = (EDamageElementType)damage.ElementType;
            damageConfig.FormulaName = damage.FormulaName;
            damageConfig.ImpactValue = damage.ImpactValue;
            damageConfig.DamageRatio = damage.DamageRatio;
            foreach (var addiId in damage.AdditiveId)
            {
                var damageAddi = ConfigManager.Table<DamageAdditiveTable>().Get(addiId);
                var funcInfo = new DamageFuncInfo();
                funcInfo.ValueFuncName = damageAddi.ApplyFuncName;
                funcInfo.ConditionIds = damageAddi.ConditionIds;
                funcInfo.ConditionParams = damageAddi.ConditionParams;
                funcInfo.ValueParams.AddRange(damageAddi.DamageValue);
                damageConfig.AddiTypes.Add(funcInfo);
            }

            foreach (var multiId in damage.MultiplyId)
            {
                var damageMultiply = ConfigManager.Table<DamageMultiplyTable>().Get(multiId);
                var funcInfo = new DamageFuncInfo();
                funcInfo.ValueFuncName = damageMultiply.ApplyFuncName;
                funcInfo.ConditionIds = damageMultiply.ConditionIds;
                funcInfo.ConditionParams = damageMultiply.ConditionParams;
                funcInfo.ValueParams.AddRange(damageMultiply.DamageValue);
                damageConfig.MultiTypes.Add(funcInfo);
            }


            var damageInfo = new DamageInfo();
            damageInfo.SourceAbilityConfigId = Ability.Context.Invoker.ConfigId;
            damageInfo.Tags.AddRange(Ability.Context.Invoker.Tags.GetAllTag());
            damageInfo.HitCount = 1;
            damageInfo.HitNumberCount = 1;
            var res = LuaInterface.GetDamageResults(attack, target, damageInfo, damageConfig);

            return res.DamageValue;
        }

        [AbilityMethod]
        public static float MakeDamage(int targetUid, int damageConfigId, int skillRate, bool isCriticalOnce)
        {
            if (!tryGetActor(targetUid, out var target))
            {
                return -1;
            }

            var damageConfig = new DamageConfig();
            var damage = ConfigManager.Table<DamageTable>().Get(damageConfigId);
            damageConfig.DamageType = (EDamageType)damage.DamageType;
            damageConfig.ElementType = (EDamageElementType)damage.ElementType;
            damageConfig.FormulaName = damage.FormulaName;
            damageConfig.ImpactValue = damage.ImpactValue;
            damageConfig.DamageRatio = damage.DamageRatio;
            foreach (var addiId in damage.AdditiveId)
            {
                var damageAddi = ConfigManager.Table<DamageAdditiveTable>().Get(addiId);
                var funcInfo = new DamageFuncInfo();
                funcInfo.ValueFuncName = damageAddi.ApplyFuncName;
                funcInfo.ConditionIds = damageAddi.ConditionIds;
                funcInfo.ConditionParams = damageAddi.ConditionParams;
                funcInfo.ValueParams.AddRange(damageAddi.DamageValue);
                damageConfig.AddiTypes.Add(funcInfo);
            }

            foreach (var multiId in damage.MultiplyId)
            {
                var damageMultiply = ConfigManager.Table<DamageMultiplyTable>().Get(multiId);
                var funcInfo = new DamageFuncInfo();
                funcInfo.ValueFuncName = damageMultiply.ApplyFuncName;
                funcInfo.ConditionIds = damageMultiply.ConditionIds;
                funcInfo.ConditionParams = damageMultiply.ConditionParams;
                funcInfo.ValueParams.AddRange(damageMultiply.DamageValue);
                damageConfig.MultiTypes.Add(funcInfo);
            }

            var damageInfo = new DamageInfo();
            damageInfo.SourceAbilityConfigId = Ability.Context.Invoker.ConfigId;
            damageInfo.Tags.AddRange(Ability.Context.Invoker.Tags.GetAllTag());
            damageInfo.HitCount = 1;
            damageInfo.HitNumberCount = 1;
            var res = LuaInterface.GetDamageResults(Ability.Context.SourceActor, target, damageInfo, damageConfig);

            return res.DamageValue;
        }

        [AbilityMethod]
        public static void AddBuff(int targetUid, int buffId, int buffLayer = 1, bool topSourceActor = false)
        {
            if (!tryGetActor(targetUid, out var target))
            {
                return;
            }

            if (target.Logic.TryGetComponent<ActorLogic.BuffComp>(out var comp))
            {
                var sourceActor = Ability.Context.SourceActor;
                var sourceUid = topSourceActor
                    ? sourceActor.GetAttr<int>(ELogicAttr.AttrSourceActorUid)
                    : sourceActor.GetAttr<int>(ELogicAttr.AttrTopSourceActorUid);
                comp.AddBuff(sourceUid, buffId, buffLayer);
            }
        }

        [AbilityMethod]
        public static void AddBuffToTargets(List<int> targetUids, int buffId, int buffLayer = 1,
            bool topSourceActor = false)
        {
            if (targetUids is not { Count: > 0 })
            {
                return;
            }

            foreach (var actorUid in targetUids)
            {
                if (!tryGetActor(actorUid, out var actor))
                {
                    return;
                }

                if (actor.Logic.TryGetComponent<ActorLogic.BuffComp>(out var comp))
                {
                    var sourceActor = Ability.Context.SourceActor;
                    var sourceUid = topSourceActor
                        ? sourceActor.GetAttr<int>(ELogicAttr.AttrSourceActorUid)
                        : sourceActor.GetAttr<int>(ELogicAttr.AttrTopSourceActorUid);
                    comp.AddBuff(sourceUid, buffId, buffLayer);
                }
            }
        }

        [AbilityMethod]
        public static void RemoveBuff(int actorUid, int buffId, int buffLayer = 1)
        {
            if (!tryGetActor(actorUid, out var actor))
            {
                return;
            }

            if (actor.Logic.TryGetComponent<ActorLogic.BuffComp>(out var comp))
            {
                comp.RemoveBuff(buffId);
            }
        }

        [AbilityMethod]
        public static void RemoveTargetsBuff(List<int> targetUids, int buffId, int buffLayer = 1)
        {
            if (targetUids is not { Count: > 0 })
            {
                return;
            }

            foreach (var actorUid in targetUids)
            {
                if (!tryGetActor(actorUid, out var actor))
                {
                    return;
                }

                if (actor.Logic.TryGetComponent<ActorLogic.BuffComp>(out var comp))
                {
                    comp.RemoveBuff(buffId);
                }
            }
        }

        [AbilityMethod]
        public static int GetBuffSoruce(int buffId)
        {
            if (Ability.Context.SourceActor.Logic.TryGetComponent<ActorLogic.BuffComp>(out var comp))
            {
                return comp.GetBuffSource(buffId);
            }

            return -1;
        }

        [AbilityMethod]
        public static void LessSkillCD(int skillId, int lessValue)
        {
            if (!tryGetActor(0, out var actor))
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

        [AbilityMethod]
        public static int GetListCount(List<int> list)
        {
            if (list == null)
            {
                Debug.LogWarning("参数为空！");
                return 0;
            }

            return list.Count;
        }

        [AbilityMethod]
        public static int GetIntListItem(List<int> list, int index)
        {
            if (list == null)
            {
                Debug.LogError("列表为空！");
                return 0;
            }

            if (index >= list.Count)
            {
                Debug.LogError("索引长度超过列表长度！");
                return 0;
            }

            return list[index];
        }

        [AbilityMethod]
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

        [AbilityMethod]
        public static void AddBattleResource(int resourceValue, bool isPer) { }

        [AbilityMethod]
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

        [AbilityMethod]
        public static bool And(bool a, bool b) => a && b;

        [AbilityMethod]
        public static bool Or(bool a, bool b) => a || b;

        [AbilityMethod]
        public static bool CompareInt(int left, ECompareResType compareType, int right)
        {
            int res = left.CompareTo(right);
            return getCompareRes(compareType, res);
        }

        [AbilityMethod]
        public static bool CompareFloat(float left, ECompareResType compareType, float right)
        {
            int res = left.CompareTo(right);
            return getCompareRes(compareType, res);
        }

        #region 数学函数

        [AbilityMethod]
        public static int IntSelfAdditive(int self) => ++self;

        [AbilityMethod]
        public static float FloatSelfAdditive(float self) => ++self;

        [AbilityMethod]
        public static int IntSelfSubtracting(int self) => --self;

        [AbilityMethod]
        public static float FloatSelfSubtracting(float self) => --self;

        #endregion
    }


    /// <summary>
    /// 私有函数
    /// </summary>
    public partial class AbilityFunction
    {
        private static bool tryGetActor(int actorUid, out Actor actor)
        {
            actor = null;
            if (actorUid <= 0)
            {
                actor = Ability.Context.SourceActor;
            }
            else
            {
                actor = ActorManager.Instance.GetActor(actorUid);
            }

            if (actor == null)
            {
                Debug.LogError($"getActor {actorUid} failed! actor is null!");
                return false;
            }

            return true;
        }

        private static bool getCompareRes(ECompareResType compareResType, int flag)
        {
            switch (compareResType)
            {
                case ECompareResType.Less:
                    return flag < 0;
                case ECompareResType.LessAndEqual:
                    return flag <= 0;
                case ECompareResType.Equal:
                    return flag == 0;
                case ECompareResType.More:
                    return flag > 0;
                case ECompareResType.MoreAndEqual:
                    return flag >= 0;
            }

            return true;
        }
    }
}