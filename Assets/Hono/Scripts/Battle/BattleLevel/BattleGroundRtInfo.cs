using System;
using System.Collections.Generic;
using System.Linq;
using Hono.Scripts.Battle.Event;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public class BattleGroundRtInfo
    {
        //阵营相关数据

        /// <summary>
        /// 当前每个阵营actor的数量
        /// </summary>
        private readonly Dictionary<int, int> _curFactionActorCount = new(16);

        /// <summary>
        /// 每波每个阵营的死亡数量
        /// </summary>
        private readonly Dictionary<int, int> _deadFactionActorCount = new(16);

        /// <summary>
        /// 整场战斗每个阵营actor的数量
        /// </summary>
        private readonly Dictionary<int, int> _deadFactionActorCountInBattle = new(16);

        //来自玩家阵营的击杀数（玩家Uid，击杀数量）每波
        private readonly Dictionary<int, int> _pawnKilledCount = new(16);

        //来自玩家阵营的击杀数（玩家Uid，击杀数量）整场战斗
        private readonly Dictionary<int, int> _pawnKilledCountInBattle = new(16);
        
        public ERoundState CurRoundState { get; set; }
        
        public float CurRoundLastTime { get; set; }

        public int CurRoundCount { get; set; }

        public int LeaderUid { get; set; }

        public int RPCount { get; set; }

        public int GetRoundSurvivalFaction(int factionId)
        {
            return _curFactionActorCount.GetValueOrDefault(factionId, 0);
        }

        public int GetRoundDeadFaction(int factionId)
        {
            return _deadFactionActorCount.GetValueOrDefault(factionId, 0);
        }

        public int GetBattleDeadFaction(int factionId)
        {
            return _deadFactionActorCount.GetValueOrDefault(factionId, 0);
        }

        public int GetRoundPawnKill(int uid)
        {
            return uid > 0 ? _pawnKilledCount.GetValueOrDefault(uid, 0) : _pawnKilledCount.Sum(pair => pair.Value);
        }

        public int GetBattlePawnKill(int uid)
        {
            return uid > 0 ? _pawnKilledCount.GetValueOrDefault(uid, 0) : _pawnKilledCount.Sum(pair => pair.Value);
        }

        /// <summary>
        /// 增加初始数量
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="configId"></param>
        public void AddFactionActorCount(EActorType actorType, int configId)
        {
            int factionId = -1;
            switch (actorType)
            {
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

            if (factionId == -1)
            {
                return;
            }

            AddFactionActorCount(factionId);
        }

        /// <summary>
        /// 增加初始数量
        /// </summary>
        /// <param name="factionId"></param>
        public void AddFactionActorCount(int factionId)
        {
            if (!_curFactionActorCount.TryAdd(factionId, 1))
            {
                _curFactionActorCount[factionId] += 1;
            }
        }

        /// <summary>
        /// 来自刷怪器的怪物死亡时的回调
        /// </summary>
        /// <param name="actor"></param>
        public void OnActorDead(Actor actor)
        {
            var factionId = actor.GetAttr<int>(ELogicAttr.AttrFaction);
            if (_curFactionActorCount.ContainsKey(factionId) && _curFactionActorCount[factionId] > 0)
            {
                --_curFactionActorCount[factionId];
                if (!_deadFactionActorCount.TryAdd(factionId, 1))
                {
                    ++_deadFactionActorCount[factionId];
                }
            }
            else
            {
                Debug.LogError("OnGenMonsterDead 清除了一个不在记录上的Monster");
            }

            if (_deadFactionActorCountInBattle.TryAdd(factionId, 1))
            {
                ++_deadFactionActorCountInBattle[factionId];
            }
        }

        /// <summary>
        /// 当Actor被杀死时
        /// </summary>
        /// <param name="beKilled"></param>
        /// <param name="damageInfo"></param>
        public void OnActorBeKilled(Actor beKilled, HitDamageInfo damageInfo)
        {
            if (ActorManager.Instance.TryGetActor(damageInfo.SourceActorId, out var actor))
            {
                if (actor.GetAttr<int>(ELogicAttr.AttrFaction) == 1)
                {
                    if (!_pawnKilledCount.TryAdd(actor.Uid, 1))
                    {
                        ++_pawnKilledCount[actor.Uid];
                    }

                    if (!_pawnKilledCountInBattle.TryAdd(actor.Uid, 1))
                    {
                        ++_pawnKilledCountInBattle[actor.Uid];
                    }
                }
            }
        }

        public void ClearRound()
        {
            _curFactionActorCount.Clear();
            _pawnKilledCount.Clear();
            _deadFactionActorCount.Clear();
        }

        public void RepeatRound()
        {
            foreach (var factionInfo in _deadFactionActorCount)
            {
                if (_deadFactionActorCountInBattle.ContainsKey(factionInfo.Key))
                {
                    _deadFactionActorCountInBattle[factionInfo.Key] -= factionInfo.Value;
                }
            }
            _deadFactionActorCount.Clear();

            foreach (var killInfo in _pawnKilledCount)
            {
                if (_pawnKilledCountInBattle.ContainsKey(killInfo.Key))
                {
                    _pawnKilledCountInBattle[killInfo.Key] -= killInfo.Value;
                }
            }
            _pawnKilledCount.Clear();
            
            _curFactionActorCount.Clear();
        }

        public void ClearAll()
        {
            _deadFactionActorCountInBattle.Clear();
            _pawnKilledCountInBattle.Clear();

            _curFactionActorCount.Clear();
            _pawnKilledCount.Clear();
            _deadFactionActorCount.Clear();
        }
    }
}