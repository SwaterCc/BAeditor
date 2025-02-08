#region

using System.Collections.Generic;
using Hono.Scripts.Battle.Core;
using Hono.Scripts.Battle.Core.Base;
using Hono.Scripts.Battle.Tools;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.AbilityFramework
{
    public class AbilitySystem : Singleton<AbilitySystem>
    {
        private readonly Dictionary<int, Dictionary<int, Ability>> _searchDict = new(30);
        private readonly List<Ability> _runningList = new(30);
        private readonly List<Ability> _removeList = new(10);
        
        public static void InitEnv()
        {
            if (!Ability.AbilityEnv.IsEnvInit)
            {
                Ability.AbilityEnv.InitEnv();
            }
        }
        
        /// <summary>
        /// 是否存在某个Ability
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool HasAbility(int id)
        {
            
            return false;
        }

        /// <summary>
        /// 赋予Ability
        /// </summary>
       
        public Ability AwardAbility(Unit unit,int id)
        {
            if (id <= 0)
            {
                Debug.LogError("CreateAbility id is 0");
                return null;
            }

            Ability ability = GPool<Ability>.Pool.Rent();
            if (!ability.Init(unit, id))
            {
                Debug.LogError($"Ability {id} Init failed!");
                GPool<Ability>.Pool.Recycle(ability);
                return null;
            }

            if (!_searchDict.TryAdd(ability.Id, ability))
            {
                Debug.LogError($"Ability {id} Add failed!");
                GPool<Ability>.Pool.Recycle(ability);
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
                GPool<Ability>.Pool.Recycle(ability);
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
                GPool<Ability>.Pool.Recycle(ability);
            }

            _runningList.Clear();
        }
    }
}