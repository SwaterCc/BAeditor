using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class ActorLogic
    {
        public class MoveState : AState
        {
            private float _baseSpeed;
            public MoveState(ActorStateMachine machine, ActorLogic actorLogicLogic) : base(machine, actorLogicLogic) { }

            protected override EActorState getStateType()
            {
                return EActorState.Move;
            }

            public override void Init()
            {
                _transDict[EActorState.Battle].Add(new AStateTransform(EActorState.Battle));
                _transDict[EActorState.Death].Add(new AStateTransform(EActorState.Death));
                _transDict[EActorState.Idle].Add(new AStateTransform(EActorState.Idle,() => _actorLogic._inputHandle.MoveInputValue.magnitude == 0));
                _transDict[EActorState.Stiff].Add(new AStateTransform(EActorState.Stiff));

                _baseSpeed = _actorLogic.GetAttr<float>(ELogicAttr.AttrBaseSpeed);
            }


            protected override void onTick(float dt)
            {
                var curPos = _actorLogic.GetAttr<Vector3>(ELogicAttr.AttrPosition);
                var curRot = Quaternion.FromToRotation(Vector3.forward, _actorLogic._inputHandle.MoveInputValue);
                var offset = InputManager.Instance.InputDirection * (_baseSpeed * dt);
                Debug.Log($"move Offset {offset}");
                _actorLogic.SetAttr(ELogicAttr.AttrPosition, curPos + offset, false);
                _actorLogic.SetAttr(ELogicAttr.AttrRot, curRot, false);
            }
        }
    }
}