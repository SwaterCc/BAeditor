#region

using Hono.Scripts.Battle.Event;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle {
	public class BattleGroundRtInfo {
		//阵营相关数据

		/// <summary>
		///     当前每个阵营actor的数量
		/// </summary>
		private readonly Dictionary<int, int> _curFactionActorCount = new(16);

		/// <summary>
		///     每波每个阵营的死亡数量
		/// </summary>
		private readonly Dictionary<int, int> _deadFactionActorCount = new(16);

		/// <summary>
		///     整场战斗每个阵营actor的数量
		/// </summary>
		private readonly Dictionary<int, int> _deadFactionActorCountInBattle = new(16);

		//来自玩家阵营的击杀数（玩家Uid，击杀数量）每波
		private readonly Dictionary<int, int> _pawnKilledCount = new(16);

		//来自玩家阵营的击杀数（玩家Uid，击杀数量）整场战斗
		private readonly Dictionary<int, int> _pawnKilledCountInBattle = new(16);

		/// <summary>
		///     当前场上的tag计数
		/// </summary>
		private readonly Dictionary<int, int> _curTagDict = new(64);

		/// <summary>
		///     当前回合死亡的Tag计数
		/// </summary>
		private readonly Dictionary<int, int> _deadTagDict = new(64);

		/// <summary>
		/// rouge模式属性拾取记录
		/// </summary>
		private readonly Dictionary<int, Dictionary<EAttrType,int>> _lootSettingsRecord = new(4);

		public ERoundState CurRoundState { get; set; }

		public float CurRoundDurationTime { get; set; }

		public int CurRoundCount { get; set; }

		public int LeaderUid { get; set; }

		public int RPCount { get; set; }

		public bool OpenAutoUlt = true;

		public int GetRoundSurvivalFaction(int factionId) {
			return _curFactionActorCount.GetValueOrDefault(factionId, 0);
		}

		public int GetRoundLastMonster() {
			int count = 0;
			foreach (var pair in _curFactionActorCount) {
				if (pair.Key != 3) continue; //临时代码
				count += pair.Value;
			}

			return count;
		}

		public int GetRougePawnAttrChange(int uid,EAttrType attrTypeType) {
			if (!_lootSettingsRecord.TryGetValue(uid, out var infoDict)) {
				return 0;
			}

			return infoDict.GetValueOrDefault(attrTypeType);
		}

		public void RecordPawnRougeAttrChange(int uid, EAttrType attrType, int value) {
			if (!_lootSettingsRecord.TryGetValue(uid, out var infoDict)) {
				infoDict = new Dictionary<EAttrType, int>();
				_lootSettingsRecord.Add(uid,infoDict);
			}
			
			infoDict[attrType] = infoDict.GetValueOrDefault(attrType) + value;
		}
		
		public int GetRoundDeadFaction(int factionId) {
			return _deadFactionActorCount.GetValueOrDefault(factionId, 0);
		}

		public int GetBattleDeadFaction(int factionId) {
			return _deadFactionActorCount.GetValueOrDefault(factionId, 0);
		}

		public int GetRoundPawnKill(int uid) {
			return uid > 0 ? _pawnKilledCount.GetValueOrDefault(uid, 0) : _pawnKilledCount.Sum(pair => pair.Value);
		}

		public int GetBattlePawnKill(int uid) {
			return uid > 0 ? _pawnKilledCount.GetValueOrDefault(uid, 0) : _pawnKilledCount.Sum(pair => pair.Value);
		}

		public int TagDeadCount(int tag) {
			return _deadTagDict.GetValueOrDefault(tag);
		}

		/// <summary>
		///     增加初始数量
		/// </summary>
		/// <param name="actorType"></param>
		/// <param name="configId"></param>
		public void AddFactionActorCount(EActorType actorType, int configId) {
			int factionId = -1;
			switch (actorType) {
				case EActorType.Pawn:
					var pawnRow = ConfigManager.Table<PawnLogicTable>().Get(configId);
					factionId = pawnRow.Faction;
					break;
				case EActorType.Monster:
					var monsterRow = ConfigManager.Table<MonsterLogicTable>().Get(configId);
					factionId = monsterRow.Faction;
					break;
				case EActorType.Building:
					var buildingRow = ConfigManager.Table<BuildingLogicTable>().Get(configId);
					factionId = buildingRow.Faction;
					break;
				default:
					Debug.Log($"[AddFactionActorCount] 数据暂时不统计{actorType}类型的对象,configId{configId}");
					return;
			}

			if (factionId == -1) {
				return;
			}

			AddFactionActorCount(factionId);
		}

		/// <summary>
		///     增加初始数量
		/// </summary>
		/// <param name="factionId"></param>
		public void AddFactionActorCount(int factionId) {
			if (!_curFactionActorCount.TryAdd(factionId, 1)) {
				_curFactionActorCount[factionId] += 1;
			}
		}

		/// <summary>
		///     来自刷怪器的怪物死亡时的回调
		/// </summary>
		/// <param name="actor"></param>
		public void OnActorDead(Actor actor) {
			var factionId = actor.GetAttr(EAttrType.AttrFaction);
			if (_curFactionActorCount.ContainsKey(factionId) && _curFactionActorCount[factionId] > 0) {
				--_curFactionActorCount[factionId];
				if (!_deadFactionActorCount.TryAdd(factionId, 1)) {
					++_deadFactionActorCount[factionId];
				}
			}

			if (_deadFactionActorCountInBattle.TryAdd(factionId, 1)) {
				++_deadFactionActorCountInBattle[factionId];
			}

			foreach (var tag in actor.TagCollection.GetAllTag()) {
				if (!_deadTagDict.TryAdd(tag, 1)) {
					++_deadTagDict[tag];
				}
			}
		}

		/// <summary>
		///     当Actor被杀死时
		/// </summary>
		/// <param name="beKilled"></param>
		/// <param name="damageInfo"></param>
		public void OnActorBeKilled(Actor beKilled, HitDamageInfo damageInfo) {
			if (ActorManager.Instance.TryGetActor(damageInfo.SourceActorId, out var actor)) {
				if (actor.GetAttr(EAttrType.AttrFaction) == 1) {
					if (!_pawnKilledCount.TryAdd(actor.Uid, 1)) {
						++_pawnKilledCount[actor.Uid];
					}

					if (!_pawnKilledCountInBattle.TryAdd(actor.Uid, 1)) {
						++_pawnKilledCountInBattle[actor.Uid];
					}
				}
			}
		}

		public void ClearRound() {
			_curFactionActorCount.Clear();
			_pawnKilledCount.Clear();
			_deadFactionActorCount.Clear();
			_deadTagDict.Clear();
		}

		public void RepeatRound() {
			foreach (var factionInfo in _deadFactionActorCount) {
				if (_deadFactionActorCountInBattle.ContainsKey(factionInfo.Key)) {
					_deadFactionActorCountInBattle[factionInfo.Key] -= factionInfo.Value;
				}
			}

			_deadFactionActorCount.Clear();

			foreach (var killInfo in _pawnKilledCount) {
				if (_pawnKilledCountInBattle.ContainsKey(killInfo.Key)) {
					_pawnKilledCountInBattle[killInfo.Key] -= killInfo.Value;
				}
			}

			_pawnKilledCount.Clear();

			_curFactionActorCount.Clear();
		}

		public void ClearAll() {
			_deadFactionActorCountInBattle.Clear();
			_pawnKilledCountInBattle.Clear();
			_deadTagDict.Clear();
			_curFactionActorCount.Clear();
			_pawnKilledCount.Clear();
			_deadFactionActorCount.Clear();
			_lootSettingsRecord.Clear();
		}
	}
}