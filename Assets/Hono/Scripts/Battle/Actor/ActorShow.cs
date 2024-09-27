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
    public abstract partial class ActorShow
    {
        public int Uid { get; }

        public Actor Actor { get; }

        /// <summary>
        /// unity中对应的对象
        /// </summary>
        protected GameObject Model { get; set; }

        /// <summary>
        /// 模型Id
        /// </summary>
        public int ModelId { get; protected set; }

        /// <summary>
        /// 表现数据
        /// </summary>
        public ModelTable.ModelRow ModelData { get; private set; }
        
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

        protected ActorLogic _actorLogic;

        protected ActorShow(Actor actor) {
	        Actor = actor;
            Uid = actor.Uid;
            _components = new Dictionary<Type, AShowComponent>();
            IsModelLoadFinish = false;
        }

        public void Setup(Tags tags,VarCollection varCollection,ActorLogic actorLogic) {
	        _tags = tags;
	        _variables = varCollection;
	        _actorLogic = actorLogic;

	        ModelId = _actorLogic.GetAttr<int>(ELogicAttr.AttrModelId);
	        if (ModelId > 0) {
		        ModelData = ConfigManager.Table<ModelTable>().Get(ModelId);
	        }
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

            IsModelLoadFinish = true;
        }

        protected virtual async UniTask loadModel() {
	        if (ModelData == null || string.IsNullOrEmpty(ModelData.ModelPath)) return;

	        try
	        {
		        Model = await Addressables.LoadAssetAsync<GameObject>(ModelData.ModelPath).ToUniTask();
		        Model = Object.Instantiate(Model);
		        if (!Model.TryGetComponent<ActorModel>(out var handle)) {
			        handle = Model.AddComponent<ActorModel>();
		        }
		        handle.ActorUid = Uid;
		        handle.ActorType = Actor.ActorType;
		        Model.name = $"{Actor.ActorType}:{Uid}";
	        }
	        catch (Exception e)
	        {
		        Debug.LogError($"加载模型失败，路径{ModelData.ModelPath}");
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

        private void registerComponents()
        {
	        
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