using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Hono.Scripts.Battle
{
    //表演层
    public abstract partial class ActorShow
    {
        public int Uid { get; }
        
        /// <summary>
        /// 属性
        /// </summary>
        protected AttrCollection _attrs;

        /// <summary>
        /// unity中对应的对象
        /// </summary>
        public GameObject Model => _gameObject;

        protected GameObject _gameObject;

        /// <summary>
        /// 表现组件
        /// </summary>
        private readonly Dictionary<Type, AShowComponent> _components;

        protected ActorShowTable.ActorShowRow _showData;
        
        protected ActorShow(int uid,ActorShowTable.ActorShowRow data)
        {
            Uid = uid;
            _components = new Dictionary<Type, AShowComponent>();
            _showData = data;
            _attrs = new AttrCollection(ShowAttrCreator.Create);
        }
        
        public async void Init()
        {
            //先load对象
            await loadModel();
            
            registerComponents();
            //对象加载完后再load组件
            foreach (var component in _components)
            {
                component.Value.Init();
            }
        }

        protected abstract UniTask loadModel();

        public void Update(float dt)
        {
            foreach (var component in _components)
            {
                component.Value.Update(dt);
            }
        }

        private void registerComponents()
        {
            registerChildComp();
        }

        protected virtual void registerChildComp() { }


        protected void addComponent(AShowComponent component)
        {
            if (!_components.TryAdd(component.GetType(), component))
            {
                Debug.Log($"{this.GetType()}  添加组件 {component.GetType()} Failed!");
            }
        }

        public T GetComponent<T>() where T : AShowComponent
        {
            if (!_components.TryGetValue(typeof(T), out var component))
            {
                Debug.Log($"{this.GetType()} 获取组件 {typeof(T)} 失败!");
            }

            return (T)component;
        }

        public void Destroy()
        {
            if (_gameObject != null)
            {
                Object.Destroy(_gameObject);
            }

            foreach (var component in _components)
            {
                component.Value.OnDestroy();
            }
        }
    }
}