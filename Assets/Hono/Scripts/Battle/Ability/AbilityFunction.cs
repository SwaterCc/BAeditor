using System.Runtime.InteropServices.ComTypes;
using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.RefValue;
using Hono.Scripts.Battle.Tools;
using Hono.Scripts.Battle.Tools.CustomAttribute;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle {
	public static partial class AbilityFunction {
		/// <summary>
		/// 编辑器默认显示函数
		/// 不允许重载函数
		/// 尽量不要在get函数里直接获取对象
		/// </summary>
		[AbilityFuncCache(EFuncCacheFlag.Action | EFuncCacheFlag.Branch | EFuncCacheFlag.Variable)]
		public static void NothingToDo() { }

		[AbilityFuncCache(EFuncCacheFlag.Action)]
		public static void SetNextStageId(int id) {
			Ability.Context.Invoker.SetNextGroupId(id);
		}

		[AbilityFuncCache(EFuncCacheFlag.Action)]
		public static void StopStage() {
			Ability.Context.Invoker.StopGroup();
		}

		[AbilityFuncCache(EFuncCacheFlag.Action)]
		public static void DebugMessage(string flag, string msg, object p1, object p2, object p3) {
#if UNITY_EDITOR
			void Convert(string pattern, params object[] args) {
				Debug.Log(string.Format(pattern, args));
			}

			string flagStr = string.IsNullOrEmpty(flag) ? "" : $"[{flag}] ";

			Convert(flagStr + msg, p1, p2, p3);
#endif
		}

		[AbilityFuncCache]
		public static EventChecker GetEmptyChecker(EBattleEventType eventType) {
			return new EmptyChecker(eventType);
		}

		[AbilityFuncCache]
		public static EventChecker GetHitChecker(EBattleEventType eventType, bool checkActor,
			bool checkAbility) {
			int actorId = checkActor ? Ability.Context.SourceActor.Uid : -1;
			int abilityId = checkAbility ? Ability.Context.Invoker.Uid : -1;
			return new HitEventChecker(null, actorId, abilityId);
		}

		[AbilityFuncCache]
		public static EventChecker GetMotionChecker(EBattleEventType eventType, int motionId) {
			return new MotionEventChecker(eventType, motionId, null);
		}

		[AbilityFuncCache(EFuncCacheFlag.Action)]
		public static void CreateHitBox(int targetUid, HitBoxData hitData) {
			var hitBox = ActorManager.Instance.CreateActor(BattleSetting.DefaultHitBoxPrototypeId);
			hitBox.SetAttr(ELogicAttr.AttrSourceActorUid, Ability.Context.SourceActor.Uid, false);
			hitBox.SetAttr(ELogicAttr.AttrSourceAbilityConfigId, Ability.Context.Invoker.ConfigId, false);
			hitBox.Variables.Set("hitBoxData", hitData);
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

		[AbilityFuncCache(EFuncCacheFlag.Action)]
		public static void AddAbility(int actorUid, int ability, bool isRunNow) {
			if (ability > 10000) {
				Debug.LogWarning($"未经过组件试图添加一个特化的Ability {ability}，这是有风险的行为,已拦截");
				return;
			}

			if (tryGetActor(actorUid, out var actor)) {
				actor?.AwardAbility(ability, isRunNow);
			}
		}

		[AbilityFuncCache(EFuncCacheFlag.Action)]
		public static void AddBuff(int actorUid, int buffId, int buffLayer = 1) {
			if (!tryGetActor(actorUid, out var actor)) {
				return;
			}

			if (actor.Logic.TryGetComponent<ActorLogic.BuffComp>(out var comp)) {
				comp.AddBuff(Ability.Context.SourceActor.Uid, buffId, buffLayer);
			}
		}

		[AbilityFuncCache(EFuncCacheFlag.Variable | EFuncCacheFlag.Branch)]
		public static object GetAttr(ELogicAttr logicAttr) {
			return Ability.Context.SourceActor.GetAttrBox(logicAttr);
		}

		[AbilityFuncCache(EFuncCacheFlag.Action)]
		public static void SetAttr(ELogicAttr logicAttr, object value, bool isTempData) {
			var command = Ability.Context.SourceActor.SetAttrBox(logicAttr, value, isTempData);
			Ability.Context.Invoker.AddCommand(command);
		}

		[AbilityFuncCache(EFuncCacheFlag.Branch | EFuncCacheFlag.Variable)]
		public static object GetVariableByUid(EVariableRange range, int actorUid,int abilityUid, string name) {
			var collections = getVariablesByUid(range, uid);

			if (collections is { Count }) {
				return collection.Get(name);
			}

			Debug.LogError("不应该走到这里");
			return default;
		}

		[AbilityFuncCache(EFuncCacheFlag.Branch | EFuncCacheFlag.Variable)]
		public static object GetVariableByTag(EVariableRange range, int tag, string name) {
			VarCollection collection = getVariablesByUid(range, tag);

			if (collection != null) {
				return collection.Get(name);
			}

			Debug.LogError("不应该走到这里");
			return default;
		}


		[AbilityFuncCache]
		public static void SetVariableByUid(EVariableRange range, int uid, string name, object valueBox) {
			VarCollection collection = getVariablesByUid(range, uid);
			collection?.Set(name, valueBox);
		}

		[AbilityFuncCache]
		public static void SetVariableByTag(EVariableRange range, int tag, string name, object valueBox) {
			VarCollection collection = getVariablesByUid(range, tag);
			collection?.Set(name, valueBox);
		}


		[AbilityFuncCache(EFuncCacheFlag.Variable | EFuncCacheFlag.Branch)]
		public static int GetSelf() {
			return Ability.Context.SourceActor.Uid;
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
	public partial class AbilityFunction {
		private static bool tryGetActor(int actorUid, out Actor actor) {
			actor = null;
			if (actorUid <= 0) {
				actor = Ability.Context.SourceActor;
			}
			else {
				actor = ActorManager.Instance.GetActor(actorUid);
			}

			if (actor == null) {
				Debug.LogError($"getActor {actorUid} failed! actor is null!");
				return false;
			}

			return true;
		}

		private static List<VarCollection> getVariablesByUid(EVariableRange range, int actorUid,int abilityUid) {
			switch (range) {
				case EVariableRange.Battleground:
					return new List<VarCollection> { BattleRoot.BattleMode.Variables };
				case EVariableRange.Actor: {
					if (tryGetActor(actorUid, out var actor)) {
						return new List<VarCollection> { actor.Variables };
					}
					break;
				}
				case EVariableRange.Ability: {
					if (tryGetActor(actorUid, out var actor)) {
						actor.GetAbility
					}
					return Ability.Context.Invoker.Variables;
				}
			}

			return null;
		}
	}
}