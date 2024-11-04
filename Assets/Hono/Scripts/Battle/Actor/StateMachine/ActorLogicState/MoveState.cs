#region

using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    public partial class ActorLogic
    {
        public class MoveState : ActorLogicState
        {
            private float _baseSpeed;
            private MotionComp _motionComp;

            public MoveState(ActorStateMachine machine, EActorLogicStateType stateType) : base(machine, stateType) { }

            public override void Init()
            {
                //_baseSpeed = _actorLogic.GetAttr<float>(ELogicAttr.AttrBaseSpeed);
                _motionComp = _actorLogic.GetComponent<MotionComp>();
            }

            protected override void onTick(float dt)
            {
                if (_motionComp is { DisableMoveInput: true })
                {
                    return;
                }

                var curPos = _actorLogic.GetAttr<Vector3>(ELogicAttr.AttrPosition);

                var finalSpeed = _actorLogic.GetAttr<float>(ELogicAttr.AttrBaseSpeed);
                var dir = _actorLogic._actorInput.MoveInputValue;
                dir.y = 0;
                var offset = dir * (finalSpeed * dt);

                if (_actorLogic.Actor.ModelController.Model.TryGetComponent<CharacterController>(out var CharCtrl))
                {
                    CharCtrl.Move(offset);
                    var finalPos = CharCtrl.transform.position;
                    finalPos.y = 0;
                    _actorLogic.SetAttr(ELogicAttr.AttrPosition, finalPos, false);
                }
                else
                {
                    var finalPos = curPos + offset;
                    finalPos.y = 0;
                    _actorLogic.SetAttr(ELogicAttr.AttrPosition, finalPos, false);
                }

                if (_motionComp is { ForceFaceMoveTarget: false })
                {
                    if (dir != Vector3.zero)
                    {
                        var curRot = Quaternion.LookRotation(_actorLogic._actorInput.MoveInputValue, Vector3.up);
                        _actorLogic.SetAttr(ELogicAttr.AttrRot, curRot, false);
                    }
                }
            }

            public override bool TryGetAutoSwitchState(out EActorLogicStateType next)
            {
                next = StateType;

                if (Mathf.Approximately(_actorLogic._actorInput.MoveInputValue.magnitude, 0))
                {
                    next = EActorLogicStateType.Idle;
                    return true;
                }

                return false;
            }
        }
    }
}