using System.Collections.Generic;
using Hono.Scripts.Battle.Core.Base;
using UnityEngine;

namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// 可移动接口，实现Move方法
    /// </summary>
    public interface IMovable
    {
        public void Move(Vector3 velocity);
    }

    /// <summary>
    /// 速度修改接口，实现最终速度获取接口
    /// </summary>
    public interface IVelocityModifier
    {
        public Vector3 GetVelocity();
    }
    
    /// <summary>
    /// 移动输入控制组件
    /// </summary>
    [JsonUnitComponent]
    public partial class MoveComp : UnitComponent
    {
        private IMovable _movable;

        private readonly List<IVelocityModifier> _velocityModifiers = new(5);

        public override void Init()
        {
            //遍历组件列表找到实现
            foreach (var comp in Unit.GetComponents().Values)
            {
                if (comp is IMovable movable)
                {
                    _movable = movable;
                }

                if (comp is IVelocityModifier velocityModifier)
                {
                    _velocityModifiers.Add(velocityModifier);
                }
            }

            if (_movable == null)
            {
                Debug.LogWarning($"[MoveComp] Unit:{Unit} 缺少可以应用移动速度的组件！");
            }
        }
        
        protected override void onTick(float dt)
        {
            var isPlayerCtrl = Unit.GetAttr(EAttrType.AttrIsPlayerCtrl);
            Vector3 baseVec = Vector3.zero;
            if (isPlayerCtrl > 0)
            {
                //玩家操控来自
                baseVec = InputManager.Instance.InputDirection.normalized *
                          (Unit.GetAttr(EAttrType.AttrMoveSpeedPCT) / 10000f);
            }
            else
            {
                //来自AI
            }

            //属性上禁止了输入移动
            if (Unit.GetAttr(EAttrType.DisableInputMove) == 0)
            {
                baseVec = Vector3.zero;
            }
            
            if (Unit.GetAttr(EAttrType.AttrSA) > 0)
            {//霸体
                _movable.Move(baseVec);
            }
            else
            {
                Vector3 finalVelocity = baseVec;
                foreach (var modifier in _velocityModifiers)
                {
                    finalVelocity += modifier.GetVelocity();
                }
                
                _movable.Move(finalVelocity);
            }
        }

        protected override void onClear()
        {
            _velocityModifiers.Clear();
            _movable = null;
        }
    }
}