using System;
using System.Collections.Generic;
using Battle.Tools;
using UnityEngine;

namespace Battle
{
    public class BattleManager : MonoBehaviour
    {
        private static BattleManager _instance;

        public static BattleManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    // 尝试查找现有实例
                    _instance = FindObjectOfType<BattleManager>();
                
                    // 如果没有找到，则创建新的 GameObject 并添加该组件
                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject(nameof(BattleManager));
                        _instance = singletonObject.AddComponent<BattleManager>();
                    }
                }

                return _instance;
            }
        }
        
        /// <summary>
        /// 获取Id生成器
        /// </summary>
        private static CommonUtility.IdGenerator _idGenerator = CommonUtility.GetIdGenerator();
        
        protected void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        
            // 检查是否已经存在另一个实例
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
            
            AbilityPreLoad.InitCache();
            AbilityDataCacheMgr.Instance.Init();
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