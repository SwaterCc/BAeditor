using Hono.Scripts.Battle.Tools;
using System.Collections.Generic;
using System;
using UnityEngine;
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
        private GameObject _model;

        public GameObject Model
        {
            get => _model;
            protected set => _model = value;
        }

        /// <summary>
        /// 表现组件
        /// </summary>
        private readonly Dictionary<Type, AShowComponent> _components;

        /// <summary>
        /// 模型是否加值完成
        /// </summary>
        public bool IsModelLoadFinish { get; protected set; }

        /// <summary>
        /// 模型来自场景
        /// </summary>
        public bool IsSceneModel { get; protected set; }

        protected Tags _tags { get; private set; }

        protected VarCollection _variables { get; private set; }

        protected ActorLogic _actorLogic { get; private set; }

        protected ActorModelController(Actor actor)
        {
            Actor = actor;
            Uid = actor.Uid;
            _components = new Dictionary<Type, AShowComponent>();
            IsModelLoadFinish = false;
        }

        /// <summary>
        /// 子类需要设置模型加载方式
        /// </summary>
        protected abstract ModelSetup getModelSetup();

        /// <summary>
        /// 组装Unity模型控制器
        /// </summary>
        /// <param name="tags"></param>
        /// <param name="varCollection"></param>
        /// <param name="actorLogic"></param>
        public void Setup(Tags tags, VarCollection varCollection, ActorLogic actorLogic)
        {
            _tags = tags;
            _variables = varCollection;
            _actorLogic = actorLogic;
            getModelSetup()?.SetupModel(this, () =>
            {
                onModelLoadFinish();
                Actor.OnModelLoadFinish?.Invoke(Actor);
            });
        }

        /// <summary>
        /// 模型加载完成
        /// </summary>
        protected virtual void onModelLoadFinish() { }

        /// <summary>
        /// ActorModel实例化到场景中
        /// </summary>
        public virtual void OnEnterScene()
        {
            if (!IsSceneModel)
            {
                _model = Object.Instantiate(_model);

                if (Model.TryGetComponent<ActorModel>(out var component))
                {
                    component.ActorType = Actor.ActorType;
                    component.ActorUid = Uid;
                }
            }
        }

        public void Update(float dt)
        {
            if (Model == null) return;

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