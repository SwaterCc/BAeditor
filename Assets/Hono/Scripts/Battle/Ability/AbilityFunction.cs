using System;
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
        [AbilityMethod]
        public static void NothingToDo() { }

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

        [AbilityMethod(false)]
        public static EventChecker GetEmptyChecker(EBattleEventType eventType)
        {
            return new EmptyChecker(eventType);
        }

        [AbilityMethod(false)]
        public static EventChecker GetHitChecker(EBattleEventType eventType, bool checkActor,
            bool checkAbility)
        {
            int actorId = checkActor ? Ability.Context.SourceActor.Uid : -1;
            int abilityId = checkAbility ? Ability.Context.Invoker.Uid : -1;
            return new HitEventChecker(null, actorId, abilityId);
        }

        [AbilityMethod(false)]
        public static EventChecker GetMotionChecker(EBattleEventType eventType, int motionId)
        {
            return new MotionEventChecker(eventType, motionId, null);
        }

        [AbilityMethod]
        public static List<int> CreateHitBoxToTargets(HitBoxData hitData)
        {
            var hitBoxUids = new List<int>();
            //返回打击点的Uid
            var targetUids = Ability.Context.SourceActor.GetAttr<List<int>>(ELogicAttr.AttrAttackTargetUids);
            if (targetUids == null)
            {
                Debug.LogWarning($"form abilityId {Ability.Context.Invoker.Uid}目标列表是空的，未创建打击点！");
                return new List<int>();
            }

            
            foreach (var targetUid in targetUids)
            {
                if (targetUid == 0) continue;
                
                var hitBox = ActorManager.Instance.CreateActor(BattleSetting.DefaultHitBoxPrototypeId);
                hitBox.SetAttr(ELogicAttr.AttrSourceActorUid, Ability.Context.SourceActor.Uid, false);
                hitBox.SetAttr(ELogicAttr.AttrSourceAbilityConfigId, Ability.Context.Invoker.ConfigId, false);
                hitBox.Variables.Set("hitBoxData", hitData);
                hitBox.Variables.Set("targetUid", targetUid);
                hitBox.Variables.Set("abilityTags", Ability.Context.Invoker.Tags.GetAllTag());
                ActorManager.Instance.AddActor(hitBox);
                hitBoxUids.Add(hitBox.Uid);
            }

            return hitBoxUids;
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

        [AbilityMethod]
        public static void SendMsg(int actorUid, string msg, int p1, int p2, int p3, int p4, int p5)
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
        public static List<int> SelectTargets(int maxSelectCount,FilterSetting setting)
        {
            Debug.LogWarning("未实现");
            return new List<int>();
        }

        [AbilityMethod]
        public static void ExecuteAbility(int actorUid, int configId)
        {
            if (tryGetActor(actorUid, out var actor))
            {
                actor.ExecuteAbilityByConfigId(configId);
            }
        }

        [AbilityMethod]
        public static int AddAbility(int actorUid, int abilityConfigId, bool isRunNow)
        {
            if (abilityConfigId > 10000)
            {
                Debug.LogWarning($"未经过组件试图添加一个特化的Ability {abilityConfigId}，这是有风险的行为,已拦截");
                return -1;
            }

            if (tryGetActor(actorUid, out var actor))
            {
                return actor.AwardAbility(abilityConfigId, isRunNow);
            }

            Debug.LogError($"添加 Ability {abilityConfigId} 失败，返回-1");
            return -1;
        }

        [AbilityMethod]
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

        [AbilityMethod]
        public static void RemoveBuff(int actorUid, int buffId, int buffLayer = 1)
        {
            if (!tryGetActor(actorUid, out var actor))
            {
                return;
            }

            if (actor.Logic.TryGetComponent<ActorLogic.BuffComp>(out var comp))
            {
                comp.RemoveByConfigId(buffId);
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
        public static bool CompareInt(int right, ECompareResType compareType, int left)
        {
            int res = right.CompareTo(left);
            return getCompareRes(compareType, res);
        }

        [AbilityMethod]
        public static bool CompareFloat(float right, ECompareResType compareType, float left)
        {
            int res = right.CompareTo(left);
            return getCompareRes(compareType, res);
        }

        #region 数学函数

        [AbilityMethod]
        public static object FloatAdd(float a, float b) => a + b;

        [AbilityMethod]
        public static int IntAdd(int a, int b) => a + b;

        [AbilityMethod]
        public static float FloatSubtract(float a, float b) => a - b;

        [AbilityMethod]
        public static int IntSubtract(int a, int b) => a - b;

        [AbilityMethod]
        public static float FloatMultiply(float a, float b) => a * b;

        [AbilityMethod]
        public static int IntMultiply(int a, int b) => a * b;

        [AbilityMethod]
        public static float FloatDivide(float a, float b) => a / b;

        [AbilityMethod]
        public static int IntDivide(int a, int b) => a / b;

        [AbilityMethod]
        public static int IntSelfAdditive(int a) => ++a;

        [AbilityMethod]
        public static float FloatSelfAdditive(float a) => ++a;

        [AbilityMethod]
        public static int IntSelfSubtracting(int a) => --a;

        [AbilityMethod]
        public static float FloatSelfSubtracting(float a) => --a;

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