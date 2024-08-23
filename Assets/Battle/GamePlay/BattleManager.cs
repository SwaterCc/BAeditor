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

        private List<Actor> _addCache = new List<Actor>();
        
        private List<IBeHurt> _beHurts = new();

        private List<int> _removeList = new List<int>();

        public void Update()
        { 
            if (_addCache.Count > 0)
            {
                foreach (var actor in _addCache)
                {
                    _actors.Add(actor.Uid, actor);
                }
                _addCache.Clear();
            }

            foreach (var actor in _actors)
            {
                actor.Value.Tick(Time.deltaTime);

                if (actor.Value.IsDisposable())
                {
                    _removeList.Add(actor.Key);
                }
            }

           
            foreach (var uid in _removeList)
            {
                _actors[uid].OnDestroy();
                _actors.Remove(uid);
            }
            _removeList.Clear();
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
            _addCache.Add(actor);
            actor.Init();
        }
    }
}