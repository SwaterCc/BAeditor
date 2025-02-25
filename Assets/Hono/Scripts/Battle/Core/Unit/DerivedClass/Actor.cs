using System;
using Hono.Scripts.Battle.Base;

namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// Actor 战斗玩法中有交互的单位
    /// </summary>
    public sealed class Actor : Unit, IGPoolObject
    {
        /// <summary>
        /// Actor基础类型
        /// </summary>
        public EActorType ActorType;

        /// <summary>
        /// model表配置
        /// </summary>
        private ModelTable.ModelRow _modelRow;
        public ModelTable.ModelRow ModelRow => _modelRow;

        /// <summary>
        /// Actor新的流程，构造流程
        /// </summary>
        public void Ctor(ActorAssembleInfo assembleInfo)
        {
            Uid = World.Current.GetUid();
            SetAttr(EAttrType.AttrUid, Uid);
            ActorType = assembleInfo.ActorType;
            Attrs.Init(assembleInfo.BaseAttrTableId);
           
            if (assembleInfo.ModelId > 0 &&
                ConfigDataBase.Table<ModelTable>().TryGet(assembleInfo.ModelId, out _modelRow))
            {
	            SetAttr(EAttrType.AttrModelId,          assembleInfo.ModelId);
	            UnitTransform.Radius = _modelRow.P1 == 0 ? 0.5f : _modelRow.P1;
                _baseSkinTemplate = _modelRow.SkinTemplateKey;
                addLoadTask(UnityAdapter.Instance.CreateUnityObjectProxy(this));
            }

            foreach (var tag in assembleInfo.Tags) {
	            Tags.Add(tag);
            }

            //添加组件
            addComponents(assembleInfo.UnitComponentBits);

            //设置组件初始值
            foreach (var pair in assembleInfo.UnitCompCtorParams)
            {
                GetComponent(pair.Key).Ctor(pair.Value);
            }
        }

        #region 周期函数

        /// <summary>
        /// 逻辑帧
        /// </summary>
        /// <param name="dt"></param>
        protected override void onTick(float dt) { }

        public override void Recycle()
        {
            GPool<Actor>.Pool.Recycle(this);
        }

        /// <summary>
        /// ActorPool回收时调用
        /// </summary>
        public void OnRecycle()
        {
            ActorType = 0;
            _modelRow = null;
        }

        #endregion
    }
}