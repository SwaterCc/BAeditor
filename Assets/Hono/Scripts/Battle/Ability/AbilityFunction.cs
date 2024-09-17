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
        public static int CreateHitBox(int targetUid, HitBoxData hitData)
        {
            if (targetUid == 0)
            {
                Debug.LogError("targetUid is 0");
                return -1;
            }

            var hitBox = ActorManager.Instance.CreateActor(BattleSetting.DefaultHitBoxPrototypeId);
            hitBox.SetAttr(ELogicAttr.AttrSourceActorUid, Ability.Context.SourceActor.Uid, false);
            hitBox.SetAttr(ELogicAttr.AttrSourceAbilityConfigId, Ability.Context.Invoker.ConfigId, false);
            hitBox.Variables.Set("hitBoxData", hitData);
            hitBox.Variables.Set("targetUid", targetUid);
            hitBox.Variables.Set("abilityTags", Ability.Context.Invoker.Tags.GetAllTag());
            ActorManager.Instance.AddActor(hitBox);
            return hitBox.Uid;
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
        public static int CalculateInt(int left, ECalculateType calculateType, int right)
        {
            Debug.LogWarning("CalculateInt没实现");
            return 0;
        }
        
        [AbilityMethod]
        public static int CalculateFloat(float left, ECalculateType calculateType, float right)
        {
            Debug.LogWarning("CalculateFloat没实现");
            return 0;
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
        public static bool CompareInt(int right, ECompareResType compareType, int left)
        {
            
            return true;
        }
        
        [AbilityMethod]
        public static bool CompareFloat(float right, ECompareResType compareType, float left)
        {
            
            return true;
        }

        #region 数学函数

        public static object FloatAdd(float a, float b) => a + b;


        public static int IntAdd(int a, int b) => a + b;


        public static float FloatSubtract(float a, float b) => a - b;


        public static int IntSubtract(int a, int b) => a - b;


        public static float FloatMultiply(float a, float b) => a * b;


        public static int IntMultiply(int a, int b) => a * b;


        public static float FloatDivide(float a, float b) => a / b;


        public static int IntDivide(int a, int b) => a / b;


        public static int IntSelfAdditive(int a) => ++a;


        public static float FloatSelfAdditive(float a) => ++a;


        public static int IntSelfSubtracting(int a) => --a;


        public static float FloatSelfSubtracting(float a) => --a;


        public static bool And(bool a, bool b) => a && b;


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