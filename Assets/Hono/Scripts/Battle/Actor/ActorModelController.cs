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
    public partial class ActorModelController
    {
        public int Uid { get; }

        public Actor Actor { get; }

        private GameObject _model;

        /// <summary>
        /// unity中对应的对象
        /// </summary>
        public GameObject Model => _model;

        /// <summary>
        /// 表现组件
        /// </summary>
        private readonly Dictionary<Type, AShowComponent> _components;

        /// <summary>
        /// 模型是否加值完成
        /// </summary>
        public bool IsModelLoadFinish { get; private set; }

        /// <summary>
        /// 模型来自场景
        /// </summary>
        public bool IsSceneModel { get; private set; }

        protected Tags _tags { get; private set; }

        protected VarCollection _variables  { get; private set; }

        protected ActorLogic _actorLogic  { get; private set; }

        public ActorModelController(Actor actor) {
	        Actor = actor;
            Uid = actor.Uid;
            _components = new Dictionary<Type, AShowComponent>();
            IsModelLoadFinish = false;
        }

        public void Setup(ModelSetup modelSetup, Tags tags, VarCollection varCollection, ActorLogic actorLogic) {
	        _tags = tags;
	        _variables = varCollection;
	        _actorLogic = actorLogic;
            modelSetup.SetupModel(this);
        }
        
        /// <summary>
        /// ActorModel实例化到场景中
        /// </summary>
        public void onEnterScene()
        {
            if (!IsSceneModel)
            {
                _model = Object.Instantiate(_model);
            }
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
                Object.Destroy(Model);
            }

            foreach (var component in _components)
            {
                component.Value.OnDestroy();
            }
        }
    }
}