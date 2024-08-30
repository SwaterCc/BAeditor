using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    //表演层
    public abstract class ActorShow
    {
        /// <summary>
        /// 配置id
        /// </summary>
        protected int _showConfigId;

        /// <summary>
        /// 属性
        /// </summary>
        protected AttrCollection _attrs;

        /// <summary>
        /// unity中对应的对象
        /// </summary>
        public GameObject Model;

        /// <summary>
        /// 逻辑组件
        /// </summary>
        protected readonly Dictionary<Type, AShowComponent> _components = new Dictionary<Type, AShowComponent>();

        public void Init()
        {
            registerComponents();
            List<IAsyncLoad> loadTask = new List<IAsyncLoad>();
            foreach (var component in _components)
            {
                component.Value.Init();
                if (component.Value is IAsyncLoad loadHandle)
                {
                    loadTask.Add(loadHandle);
                }
            }

            //先load对象
            
            //对象加载完后再load组件
        }

        public async void AsyncLoadStart() { }

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
    }
}