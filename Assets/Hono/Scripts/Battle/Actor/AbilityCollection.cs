#region

using System.Collections.Generic;
using Hono.Scripts.Battle.AbilitySystem;
using Hono.Scripts.Battle.Tools;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    public class AbilityCollection
    {
        private readonly Actor _actor;
        private readonly Dictionary<int, Ability> _searchDict = new(30);
        private readonly List<Ability> _runningList = new(30);
        private readonly List<int> _removeList = new(10);

        public AbilityCollection(Actor actor)
        {
            _actor = actor;
        }

        /// <summary>
        /// 是否存在某个Ability
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool HasAbility(int id)
        {
            foreach (var ability in _searchDict)
            {
                if (ability.Value.Id == id)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 赋予Ability
        /// </summary>
        /// <param name="id"></param>
        public Ability AwardAbility(int id)
        {
            if (id <= 0)
            {
                Debug.LogError("CreateAbility id is 0");
                return null;
            }

            Ability ability = APool<Ability>.Pool.Rent();
            if (!ability.Init(_actor, id))
            {
                Debug.LogError($"Ability {id} Init failed!");
                APool<Ability>.Pool.Recycle(ability);
                return null;
            }

            if (!_searchDict.TryAdd(ability.Id, ability))
            {
                Debug.LogError($"Ability {id} Add failed!");
                APool<Ability>.Pool.Recycle(ability);
                return null;
            }

            _runningList.Add(ability);
            return ability;
        }

        public void Tick(float dt)
        {
            int i = 0;
            while (i++ < _runningList.Count)
            {
                _runningList[i].OnTick(dt);
            }

            foreach (var id in _removeList)
            {
                if (!_searchDict.Remove(id, out Ability ability))
                {
                    continue;
                }

                _runningList.RemoveSwapBack(ability);
                APool<Ability>.Pool.Recycle(ability);
            }

            _removeList.Clear();
        }

        /// <summary>
        /// 移除Ability
        /// </summary>
        /// <param name="configId"></param>
        public void RemoveAbility(int configId)
        {
            if (_searchDict.ContainsKey(configId))
            {
                _removeList.Add(configId);
            }
        }

        /// <summary>
        /// 清理
        /// </summary>
        public void Clear()
        {
            _searchDict.Clear();
            _removeList.Clear();
            foreach (var ability in _runningList)
            {
                APool<Ability>.Pool.Recycle(ability);
            }

            _runningList.Clear();
        }
    }
}