using System;
using System.Collections.Generic;
using Battle.Tools;
using UnityEngine;

namespace Battle
{
    public class BattleManager : MonoBehaviour
    {
        public static BattleManager Instance;
        
        /// <summary>
        /// 获取Id生成器
        /// </summary>
        private static CommonUtility.IdGenerator _idGenerator = CommonUtility.GetIdGenerator();
        
        protected void Awake()
        {
            Instance ??= this;
        }
        
        private Dictionary<int, Actor> _actors = new();

        private List<IBeHurt> _beHurts = new();

        public void Update()
        {
            foreach (var actor in _actors)
            {
                actor.Value.Tick(Time.deltaTime);
            }
        }

        public Actor GetActor(int id)
        {
            if (_actors.TryGetValue(id, out var actor))
            {
                return actor;
            }

            return null;
        }
        
        public void Add(Actor actor)
        {
            var uid = _idGenerator.GenerateId();
            actor.Uid = uid;
            _actors.Add(uid, actor);
        }
    }
}