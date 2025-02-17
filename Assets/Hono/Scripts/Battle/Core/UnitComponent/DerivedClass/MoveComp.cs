using System.Collections.Generic;
using Hono.Scripts.Battle.Core.Base;
using UnityEngine;

namespace Hono.Scripts.Battle.Core
{
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
    public partial class MoveComp : UnitComponent, IGPoolObject
    {
        private readonly List<IVelocityModifier> _velocityModifiers = new(5);

        public override void Init()
        {
            //遍历组件列表找到实现
            foreach (var comp in Unit.GetComponents().Values)
            {
                if (comp is IVelocityModifier velocityModifier)
                {
                    _velocityModifiers.Add(velocityModifier);
                }
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
            if (Unit.GetAttr(EAttrType.DisableInputMove) > 0)
            {
                baseVec = Vector3.zero;
            }

            if (Unit.GetAttr(EAttrType.AttrSA) > 0)
            {
                //霸体
                move(baseVec, dt);
            }
            else
            {
                Vector3 finalVelocity = baseVec;
                foreach (var modifier in _velocityModifiers)
                {
                    finalVelocity += modifier.GetVelocity();
                }

                move(finalVelocity, dt);
            }
        }

        private void move(Vector3 velocity, float dt)
        {
            if (UnityAdapter.Instance.TryGetUnityObjectProxy(Unit.Uid, out var proxy))
            {
                Unit.UnitTransform.Pos = proxy.SyncVelocity(velocity, dt);
            }
            else
            {
                Unit.UnitTransform.Pos += velocity * dt;
            }
        }

        public override void Recycle()
        {
            GPool<MoveComp>.Pool.Recycle(this);
        }

        public void OnRecycle()
        {
            _velocityModifiers.Clear();
        }
    }
}