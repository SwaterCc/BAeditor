using UnityEngine;

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
                _baseSpeed = _actorLogic.GetAttr<float>(ELogicAttr.AttrBaseSpeed);
                _motionComp = _actorLogic.GetComponent<MotionComp>();
            }

            protected override void onTick(float dt)
            {
                if (_motionComp is { DisableMoveInput: true })
                {
                    return;
                }
                
                var curPos = _actorLogic.GetAttr<Vector3>(ELogicAttr.AttrPosition);
                var curRot = Quaternion.LookRotation( _actorLogic._actorInput.MoveInputValue,Vector3.up);
                var offset = _actorLogic._actorInput.MoveInputValue * (_baseSpeed * dt);
                _actorLogic.SetAttr(ELogicAttr.AttrPosition, curPos + offset, false);

                if (_motionComp is { ForceFaceMoveTarget: false })
                {
                    _actorLogic.SetAttr(ELogicAttr.AttrRot, curRot, false);
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