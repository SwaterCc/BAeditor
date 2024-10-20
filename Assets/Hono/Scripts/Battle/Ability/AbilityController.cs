using Hono.Scripts.Battle.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class Actor
    {
        public class AbilityController
        {
            private readonly Actor _actor;
            private readonly Dictionary<int, Ability> _abilities = new(64);
            private readonly List<Ability> _addUidCache = new(32);
            private readonly List<int> _removeUids = new(16);

            public AbilityController(Actor actor)
            {
                _actor = actor;
            }

            public bool HasAbility(int configId)
            {
                foreach (var ability in _abilities)
                {
                    if (ability.Value.ConfigId == configId)
                        return true;
                }

                return false;
            }

            public bool TryGetAbility(int uid, out Ability ability)
            {
                return _abilities.TryGetValue(uid, out ability);
            }

            public Ability CreateAbility(int configId)
            {
                if (configId <= 0)
                {
                    Debug.LogError("CreateAbility id is 0");
                    return null;
                }

                var ability = new Ability(_actor, configId);
                return ability;
            }

            public void AwardAbility(Ability ability, bool isRunNow)
            {
                if (ability == null)
                {
                    Debug.LogError("AwardAbility ability is null");
                    return;
                }

                if (isRunNow) ability.Execute();
                _addUidCache.Add(ability);
            }

            /// <summary>
            /// 赋予能力
            /// </summary>
            /// <param name="configId"></param>
            /// <param name="isRunNow"></param>
            public int AwardAbility(int configId, bool isRunNow)
            {
                if (configId <= 0)
                {
                    Debug.LogError("CreateAbility id is 0");
                    return -1;
                }

                var ability = new Ability(_actor, configId);
                _addUidCache.Add(ability);
                if (isRunNow)
                    ability.Execute();
                return ability.Uid;
            }

            /// <summary>
            /// 执行指定能力，会从资源检测开始，未Init的会主动调用一次Init
            /// </summary>
            public void ExecutingAbility(int configId)
            {
                if (_abilities.TryGetValue(configId, out var ability))
                {
                    ability.Execute();
                }
            }

            public void ExecutingAbilityForce(int configId)
            {
                if (_abilities.TryGetValue(configId, out var ability))
                {
                    ability.Stop();
                    ability.Execute();
                }
            }

            public void RemoveAbility(int configId)
            {
                if (_abilities.ContainsKey(configId))
                {
                    _removeUids.Add(configId);
                }
            }

            public void Tick(float dt)
            {
                foreach (var uid in _removeUids)
                {
                    if (_abilities.TryGetValue(uid, out var ability))
                    {
                        ability.OnDestroy();
                        _abilities.Remove(uid);
                    }
                }

                _removeUids.Clear();

                foreach (var ability in _addUidCache)
                {
                    if (!_abilities.TryAdd(ability.Uid, ability))
                    {
                        Debug.Log($"AbilityUid {ability.Uid} 重复添加");
                    }
                }

                _addUidCache.Clear();

                foreach (var abilityPair in _abilities)
                {
                    abilityPair.Value.OnTick(dt);
                }
            }

            public void Clear()
            {
                _addUidCache.Clear();
                _removeUids.Clear();
                foreach (var ability in _abilities)
                {
                    ability.Value.OnDestroy();
                }
                _abilities.Clear();
            }
        }
    }
}