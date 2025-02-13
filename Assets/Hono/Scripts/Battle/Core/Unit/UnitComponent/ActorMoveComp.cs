using UnityEngine;

namespace Hono.Scripts.Battle.Core
{
    //外部产生输入
    //Action带来的Action速度
    //输入转换为带方向的速度

    //计算最终速度
    //产生移动

    /// <summary>
    /// 移动输入控制组件
    /// </summary>
    public class ActorMoveComp : UnitComponent
    {
        private Actor Actor { get; }

        /// <summary>
        /// 最终移动方向
        /// </summary>
        private Vector3 _velocity;

        public ActorMoveComp(ComponentCtorParams ctorParams) : base(ctorParams)
        {
            Actor = (Actor)Unit;
        }

        public override void Init() { }

        public void AddVelocity(Vector3 velocity)
        {
            if (Actor.GetAttr(EAttrType.AttrSA) > 0)
            {
                return;
            }
            _velocity += velocity;
        }

        public void RemoveVelocity(Vector3 velocity)
        {
            if (Actor.GetAttr(EAttrType.AttrSA) > 0)
            {
                return;
            }
            _velocity -= velocity;
        }

        protected override void onTick(float dt)
        {
            var isPlayerCtrl = Actor.GetAttr(EAttrType.AttrIsPlayerCtrl);
            Vector3 baseVec = Vector3.zero;
            if (isPlayerCtrl > 0)
            {
                //玩家操控来自
                baseVec = InputManager.Instance.InputDirection.normalized * (Actor.GetAttr(EAttrType.AttrMoveSpeedPCT) / 10000f);
            }
            else
            {
                //来自AI
            }

            //属性上禁止了输入移动
            if (Actor.GetAttr(EAttrType.DisableInputMove) == 0)
            {
                baseVec = Vector3.zero;
            }

            var finalVelocity = _velocity + baseVec;
            Actor.ModelController.CharCtrl.SimpleMove(finalVelocity);
        }

        protected override void onClear()
        {
            _velocity = Vector3.zero;
        }
    }
}