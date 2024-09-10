using Cysharp.Threading.Tasks;
using Hono.Scripts.Battle.Tools;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    //表演层
    public abstract partial class ActorShow
    {
        public int Uid { get; }

        public Actor Actor { get; }

        /// <summary>
        /// unity中对应的对象
        /// </summary>
        public GameObject Model { get; protected set; }

        /// <summary>
        /// 表现数据
        /// </summary>
        public ActorShowTable.ActorShowRow ShowData { get; }
        
        /// <summary>
        /// 表现组件
        /// </summary>
        private readonly Dictionary<Type, AShowComponent> _components;

        /// <summary>
        /// 模型是否加值完成
        /// </summary>
        public bool IsModelLoadFinish { get; protected set; }

        protected Tags _tags;

        protected VarCollection _variables;

        protected ActorRTState _rtState;

        protected ActorShow(Actor actor,ActorShowTable.ActorShowRow data) {
	        Actor = actor;
            Uid = actor.Uid;
            _components = new Dictionary<Type, AShowComponent>();
            ShowData = data;
            IsModelLoadFinish = false;
        }

        public void Setup(Tags tags,VarCollection varCollection,ActorRTState rtState) {
	        _tags = tags;
	        _variables = varCollection;
	        _rtState = rtState;
        }
        
        public async void Init()
        {
            //先load对象
            await loadModel();
            
            registerComponents();
            registerChildComponents();
            
            //对象加载完后再load组件
            foreach (var component in _components)
            {
                component.Value.Init();
            }

            IsModelLoadFinish = true;
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
         
        }

        protected virtual void registerChildComponents() { }


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
            if (Model != null)
            {
                GameObject.Destroy(Model);
            }

            foreach (var component in _components)
            {
                component.Value.OnDestroy();
            }
        }
    }
}