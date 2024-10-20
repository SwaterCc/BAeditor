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
        public int Uid { get; private set; }

        public Actor Actor { get; private set; }

        public GameObject Model { get; protected set; }

        public bool IsModelLoadFinish { get; private set; }

        protected Tags _tags { get; private set; }

        protected VarCollection _variables { get; private set; }

        protected ActorLogic _actorLogic { get; private set; }
        
        private ModelSetup _modelSetup;

        public void Init(Actor actor)
        {
            Actor = actor;
            Uid = actor.Uid;
            IsModelLoadFinish = false;
        }

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
            _modelSetup = getModelSetup();
            _modelSetup?.LoadModel(this, modelLoadComplete);
        }

        protected abstract ModelSetup getModelSetup();

        private void modelLoadComplete(GameObject model)
        {
            Model = model;
            IsModelLoadFinish = true;
            onModelLoadComplete();
            Actor.OnModelLoadFinish?.Invoke(Actor);
        }

        protected virtual void onModelLoadComplete() { }

        /// <summary>
        /// ActorModel实例化到场景中
        /// </summary>
        public void EnterScene()
        {
            onEnterScene();
        }

        protected virtual void onEnterScene() { }

        public void Update(float dt)
        {
            if (Model == null) return;

            Model.transform.localPosition = Actor.GetAttr<Vector3>(ELogicAttr.AttrPosition);
            Model.transform.localRotation = Actor.GetAttr<Quaternion>(ELogicAttr.AttrRot);
        }

        public void Destroy()
        {
            RecycleSelf();
            Uid = 0;
            Model = null;
            _variables = null;
            _actorLogic = null;
            _tags = null;
            _modelSetup?.UnInit();
        }

        protected abstract void RecycleSelf();
    }
}