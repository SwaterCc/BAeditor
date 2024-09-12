using System.Collections;
using System.Runtime.InteropServices.ComTypes;
using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.RefValue;
using Hono.Scripts.Battle.Tools;
using Hono.Scripts.Battle.Tools.CustomAttribute;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public static partial class AbilityFunction
    {
        /// <summary>
        /// 编辑器默认显示函数
        /// 不允许重载函数
        /// 尽量不要在get函数里直接获取对象
        /// </summary>
        [AbilityFuncCache(EFuncCacheFlag.Action | EFuncCacheFlag.Branch | EFuncCacheFlag.Variable)]
        public static void NothingToDo() { }

        [AbilityFuncCache(EFuncCacheFlag.Action)]
        public static void SetNextStageId(int id)
        {
            Ability.Context.Invoker.SetNextGroupId(id);
        }

        [AbilityFuncCache(EFuncCacheFlag.Action)]
        public static void StopStage()
        {
            Ability.Context.Invoker.StopGroup();
        }

        [AbilityFuncCache(EFuncCacheFlag.Action)]
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

        [AbilityFuncCache]
        public static EventChecker GetEmptyChecker(EBattleEventType eventType)
        {
            return new EmptyChecker(eventType);
        }

        [AbilityFuncCache]
        public static EventChecker GetHitChecker(EBattleEventType eventType, bool checkActor,
            bool checkAbility)
        {
            int actorId = checkActor ? Ability.Context.SourceActor.Uid : -1;
            int abilityId = checkAbility ? Ability.Context.Invoker.Uid : -1;
            return new HitEventChecker(null, actorId, abilityId);
        }

        [AbilityFuncCache]
        public static EventChecker GetMotionChecker(EBattleEventType eventType, int motionId)
        {
            return new MotionEventChecker(eventType, motionId, null);
        }
        
        [AbilityFuncCache(EFuncCacheFlag.Action)]
        public static void CreateHitBox(int targetUid, HitBoxData hitData)
        {
            var hitBox = ActorManager.Instance.CreateActor(BattleSetting.DefaultHitBoxPrototypeId);
            hitBox.SetAttr(ELogicAttr.AttrSourceActorUid, Ability.Context.SourceActor.Uid, false);
            hitBox.SetAttr(ELogicAttr.AttrSourceAbilityConfigId, Ability.Context.Invoker.ConfigId, false);
            hitBox.Variables.Set("hitBoxData", hitData);
            hitBox.Variables.Set("targetUid", targetUid);
            hitBox.Variables.Set("abilityTags",Ability.Context.Invoker.Tags.GetAllTag());
            ActorManager.Instance.AddActor(hitBox);
        }

        /*[AbilityFuncCache(EFuncCacheFlag.Action)]
        public static void CreateBullet(int hitDataId)
        {
            var hitBox = ActorManager.Instance.CreateActor(hitDataId);
            hitBox.Logic.SetAttr(ELogicAttr.AttrSourceActorUid, Ability.Context.BelongActor.Uid, false);
            hitBox.Logic.SetAttr(ELogicAttr.SourceAbilityType, Ability.Context.Invoker.AbilityData.Type, false);
            hitBox.Logic.SetAttr(ELogicAttr.AttrSourceAbilityConfigId, Ability.Context.Invoker.AbilityData.Type, false);
            ActorManager.Instance.AddActor(hitBox);
        }*/
        [AbilityFuncCache(EFuncCacheFlag.Variable)]
        public static int GetAbilityUid(EAbilityType abilityType, int actorUid, int configId, int sourceId)
        {
            return 0;
        }

        [AbilityFuncCache(EFuncCacheFlag.Action)]
        public static void ExecuteAbility(AutoValue actorUid,int configId)
        {
            if (tryGetActor(actorUid, out var actor))
            {
                actor.ExecuteAbilityByConfigId(configId);
            }
        }
        
        [AbilityFuncCache(EFuncCacheFlag.Action | EFuncCacheFlag.Variable)]
        public static int AddAbility(int actorUid, int ability, bool isRunNow)
        {
            if (ability > 10000)
            {
                Debug.LogWarning($"未经过组件试图添加一个特化的Ability {ability}，这是有风险的行为,已拦截");
                return -1;
            }

            if (tryGetActor(actorUid, out var actor))
            {
                return actor.AwardAbility(ability, isRunNow);
            }

            Debug.LogError($"添加 Ability {ability} 失败，返回-1");
            return -1;
        }

        [AbilityFuncCache(EFuncCacheFlag.Action)]
        public static void AddBuff(int actorUid, int buffId, int buffLayer = 1)
        {
            if (!tryGetActor(actorUid, out var actor))
            {
                return;
            }

            if (actor.Logic.TryGetComponent<ActorLogic.BuffComp>(out var comp))
            {
                comp.AddBuff(Ability.Context.SourceActor.Uid, buffId, buffLayer);
            }
        }

        [AbilityFuncCache(EFuncCacheFlag.Variable | EFuncCacheFlag.Branch)]
        public static object GetAttr(int actorUid, ELogicAttr logicAttr)
        {
            if (!tryGetActor(actorUid, out var actor))
            {
                return null;
            }

            return actor.GetAttrBox(logicAttr);
        }

        [AbilityFuncCache(EFuncCacheFlag.Action)]
        public static void SetAttr(int actorUid, ELogicAttr logicAttr, object value, bool isTempData)
        {
            if (!tryGetActor(actorUid, out var actor))
            {
                return;
            }

            var command = actor.SetAttrBox(logicAttr, value, isTempData);
            if (isTempData && command != null)
                Ability.Context.Invoker.AddCommand(command);
        }

        [AbilityFuncCache(EFuncCacheFlag.Branch | EFuncCacheFlag.Variable)]
        public static object GetVariableByUid(EVariableRange range, int actorUid, int abilityUid, string name)
        {
            var collection = getVariablesByUid(range, actorUid, abilityUid);

            if (collection != null)
            {
                return collection.Get(name);
            }

            Debug.LogError("获取Variable失败");
            return default;
        }

        [AbilityFuncCache]
        public static void SetVariableByUid(EVariableRange range, int actorUid, int abilityUid, string name,
            object valueBox)
        {
            VarCollection collection = getVariablesByUid(range, actorUid, abilityUid);
            if (collection == null)
            {
                Debug.LogError("获取Variable失败");
                return;
            }

            collection?.Set(name, valueBox);
        }

        [AbilityFuncCache(EFuncCacheFlag.Variable | EFuncCacheFlag.Branch)]
        public static int GetSelf()
        {
            return Ability.Context.SourceActor.Uid;
        }

        [AbilityFuncCache(EFuncCacheFlag.Variable)]
        public static int CalculateFloat(float left, ECalculateType calculateType, float right)
        {
            Debug.LogWarning("CalculateFloat没实现");
            return 0;
        }

        [AbilityFuncCache(EFuncCacheFlag.Variable)]
        public static int CalculateInt(int left, ECalculateType calculateType, int right)
        {
            Debug.LogWarning("CalculateInt没实现");
            return 0;
        }

        [AbilityFuncCache(EFuncCacheFlag.Variable)]
        public static int GetListCount(object list)
        {
            if (list == null || list is not IList iList)
            {
                Debug.LogWarning("参数为空！");
                return 0;
            }

            return iList.Count;
        }

        [AbilityFuncCache(EFuncCacheFlag.Variable)]
        public static object GetListValueByIndex(object list, int idx)
        {
            if (list == null || list is not IList iList ||iList.Count <= idx)
            {
                Debug.LogWarning("索引超过了列表长度");
                return null;
            }

            return iList[idx];
        }

        #region 数学函数

        [AbilityFuncCache(EFuncCacheFlag.Variable)]
        public static object FloatAdd(float a, float b) => a + b;

        [AbilityFuncCache(EFuncCacheFlag.Variable)]
        public static int IntAdd(int a, int b) => a + b;

        [AbilityFuncCache(EFuncCacheFlag.Variable)]
        public static float FloatSubtract(float a, float b) => a - b;

        [AbilityFuncCache(EFuncCacheFlag.Variable)]
        public static int IntSubtract(int a, int b) => a - b;

        [AbilityFuncCache(EFuncCacheFlag.Variable)]
        public static float FloatMultiply(float a, float b) => a * b;

        [AbilityFuncCache(EFuncCacheFlag.Variable)]
        public static int IntMultiply(int a, int b) => a * b;

        [AbilityFuncCache(EFuncCacheFlag.Variable)]
        public static float FloatDivide(float a, float b) => a / b;

        [AbilityFuncCache(EFuncCacheFlag.Variable)]
        public static int IntDivide(int a, int b) => a / b;

        [AbilityFuncCache(EFuncCacheFlag.Variable)]
        public static int IntSelfAdditive(int a) => ++a;

        [AbilityFuncCache(EFuncCacheFlag.Variable)]
        public static float FloatSelfAdditive(float a) => ++a;

        [AbilityFuncCache(EFuncCacheFlag.Variable)]
        public static int IntSelfSubtracting(int a) => --a;

        [AbilityFuncCache(EFuncCacheFlag.Variable)]
        public static float FloatSelfSubtracting(float a) => --a;

        [AbilityFuncCache(EFuncCacheFlag.Variable)]
        public static bool And(bool a, bool b) => a && b;

        [AbilityFuncCache(EFuncCacheFlag.Variable)]
        public static bool Or(bool a, bool b) => a || b;

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

        private static VarCollection getVariablesByUid(EVariableRange range, int actorUid, int abilityUid)
        {
            switch (range)
            {
                case EVariableRange.Battleground:
                    return BattleRoot.BattleMode.Variables;
                case EVariableRange.Actor:
                {
                    if (tryGetActor(actorUid, out var actor))
                    {
                        return actor.Variables;
                    }

                    break;
                }
                case EVariableRange.Ability:
                {
                    if (tryGetActor(actorUid, out var actor))
                    {
                        if (actorUid <= 0)
                        {
                            return Ability.Context.Invoker.Variables;
                        }

                        if (actor.TryGetAbility(abilityUid, out var ability))
                        {
                            return ability.Variables;
                        }
                    }

                    return null;
                }
            }

            return null;
        }
    }
}