using Cysharp.Threading.Tasks;
using Hono.Scripts.Battle.Tools;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Hono.Scripts.Battle
{
    //表演层,非引擎逻辑与引擎逻辑的接口，负责管理GameObject的创建与删除
    public abstract partial class ActorModelController
    {
        public int Uid { get; }

        public Actor Actor { get; }

        /// <summary>
        /// unity中对应的对象
        /// </summary>
        protected GameObject Model { get; set; }

        /// <summary>
        /// 模型组装类型
        /// </summary>
        protected ModelSetup _modelSetup;
        
        /// <summary>
        /// 表现组件
        /// </summary>
        private readonly Dictionary<Type, AShowComponent> _components;

        /// <summary>
        /// 模型是否加值完成
        /// </summary>
        public bool IsModelLoadFinish { get; protected set; }

        protected Tags _tags { get; private set; }

        protected VarCollection _variables  { get; private set; }

        protected ActorLogic _actorLogic  { get; private set; }

        protected ActorModelController(Actor actor, ModelSetup modelSetup) {
	        Actor = actor;
            Uid = actor.Uid;
            _components = new Dictionary<Type, AShowComponent>();
            _modelSetup = modelSetup;
            IsModelLoadFinish = false;
        }

        public void Setup(Tags tags,VarCollection varCollection,ActorLogic actorLogic) {
	        _tags = tags;
	        _variables = varCollection;
	        _actorLogic = actorLogic;
            _modelSetup.SetupModel();
        }
        
        public async void Init()
        {
	        
        }

        public void Update(float dt)
        {
	        if(Model == null) return;

	        Model.transform.localPosition = Actor.GetAttr<Vector3>(ELogicAttr.AttrPosition);
	        Model.transform.localRotation = Actor.GetAttr<Quaternion>(ELogicAttr.AttrRot);
	        
            foreach (var component in _components)
            {
                component.Value.Update(dt);
            }
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