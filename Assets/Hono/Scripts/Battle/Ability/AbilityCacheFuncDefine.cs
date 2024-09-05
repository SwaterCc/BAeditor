using System.Runtime.InteropServices.ComTypes;
using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.RefValue;
using Hono.Scripts.Battle.Tools.CustomAttribute;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public static class AbilityCacheFuncDefine
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
            Ability.Context.CurrentAbility.SetNextGroupId(id);
        }

        [AbilityFuncCache(EFuncCacheFlag.Action)]
        public static void StopStage()
        {
            Ability.Context.CurrentAbility.StopGroup();
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
        public static EventChecker GetHitChecker(EBattleEventType eventType, int hitId, bool checkActor,
            bool checkAbility)
        {
            int actorId = checkActor ? Ability.Context.CurrentAbility.BelongActorId : -1;
            int abilityId = checkAbility ? Ability.Context.CurrentAbility.Uid : -1;
            return new HitEventChecker(null, hitId, actorId, abilityId);
        }

        [AbilityFuncCache]
        public static EventChecker GetMotionChecker(EBattleEventType eventType, int motionId)
        {
            return new MotionEventChecker(eventType, motionId, null);
        }

        [AbilityFuncCache(EFuncCacheFlag.Action)]
        public static void CreateHitBox(int hitDataId)
        {
            var hitBox = ActorManager.Instance.CreateActor(hitDataId);
            hitBox.Logic.SetAttr(ELogicAttr.AttrSourceActorUid, Ability.Context.BelongActor.Uid, false);
            hitBox.Logic.SetAttr(ELogicAttr.SourceAbilityType, Ability.Context.CurrentAbility.AbilityData.Type, false);
            hitBox.Logic.SetAttr(ELogicAttr.AttrSourceAbilityUid, Ability.Context.CurrentAbility.AbilityData.Type,
                false);
            ActorManager.Instance.AddActor(hitBox);
        }

        /*[AbilityFuncCache(EFuncCacheFlag.Action)]
        public static void CreateBullet(int hitDataId)
        {
            var hitBox = ActorManager.Instance.CreateActor(hitDataId);
            hitBox.Logic.SetAttr(ELogicAttr.AttrSourceActorUid, Ability.Context.BelongActor.Uid, false);
            hitBox.Logic.SetAttr(ELogicAttr.SourceAbilityType, Ability.Context.CurrentAbility.AbilityData.Type, false);
            hitBox.Logic.SetAttr(ELogicAttr.AttrSourceAbilityUid, Ability.Context.CurrentAbility.AbilityData.Type, false);
            ActorManager.Instance.AddActor(hitBox);
        }*/

        [AbilityFuncCache(EFuncCacheFlag.Action)]
        public static void AddAbility(int actorUid, int ability, bool isRunNow)
        {
            if (ability > 10000)
            {
                Debug.LogWarning($"未经过组件试图添加一个特化的Ability {ability}，这是有风险的行为");
            }

            var actor = ActorManager.Instance.GetActor(actorUid);
            actor?.Logic.AwardAbility(ability, isRunNow);
        }

        [AbilityFuncCache(EFuncCacheFlag.Action)]
        public static void AddBuff(int actorUid, int buffId, int buffLayer = 1)
        {
            var actor = ActorManager.Instance.GetActor(actorUid);
            if (actor.Logic.TryGetComponent<ActorLogic.BuffComp>(out var comp))
            {
                comp.AddBuff(Ability.Context.BelongActor.Uid, buffId, buffLayer);
            }
        }

        [AbilityFuncCache(EFuncCacheFlag.Variable | EFuncCacheFlag.Branch)]
        public static object GetLogicAttr(ELogicAttr logicAttr)
        {
            return Ability.Context.BelongLogic.GetAttrBox(logicAttr);
        }

        [AbilityFuncCache(EFuncCacheFlag.Action)]
        public static void SetLogicAttr(ELogicAttr logicAttr, object value, bool isTempData)
        {
            var command = Ability.Context.BelongLogic.SetAttrBox(logicAttr, value, isTempData);
            Ability.Context.CurrentAbility.AddCommand(command);
        }

        public static Variables GetVariableCollection(EVariableRange range)
        {
            switch (range)
            {
                case EVariableRange.Battleground:
                    return default;
                case EVariableRange.Actor:
                    return Ability.Context.BelongLogic.GetVariables();
                case EVariableRange.Ability:
                    return Ability.Context.CurrentAbility.GetVariables();
            }

            return null;
        }

        [AbilityFuncCache(EFuncCacheFlag.Branch | EFuncCacheFlag.Variable)]
        public static object GetVariable(EVariableRange range, string name)
        {
            Variables collection = GetVariableCollection(range);

            if (collection != null)
            {
                return collection.GetVariable(name);
            }

            Debug.LogError("不应该走到这里");
            return default;
        }

        [AbilityFuncCache(EFuncCacheFlag.OnlyCache)]
        public static void ChangeVariable(EVariableRange range, string name, object valueBox)
        {
            Variables collection = GetVariableCollection(range);
            collection.ChangeValue(name, valueBox);
        }

        [AbilityFuncCache(EFuncCacheFlag.OnlyCache)]
        public static void CreateVariable(EVariableRange range, string name, object valueBox)
        {
            Variables collection = GetVariableCollection(range);
            collection?.Add(name, valueBox);
        }

        [AbilityFuncCache(EFuncCacheFlag.Variable | EFuncCacheFlag.Branch)]
        public static int GetActorSelf()
        {
            return Ability.Context.CurrentAbility.BelongActorId;
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
}