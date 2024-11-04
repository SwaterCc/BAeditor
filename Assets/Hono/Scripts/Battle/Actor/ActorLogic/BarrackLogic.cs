#region

using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    public class BarrackLogic : BuildingLogic
    {
	    /// <summary>
	    ///     最大的士兵数量
	    /// </summary>
	    private int _maxSoldierCount = 5;

	    /// <summary>
	    ///     当前的士兵数量
	    /// </summary>
	    private int _curSoldierCount;

	    /// <summary>
	    ///     生产士兵的时间
	    /// </summary>
	    private const float ProduceSoldierTime = 3f;

	    /// <summary>
	    ///     生产间隔
	    /// </summary>
	    private float _produceInterval = 0;

	    /// <summary>
	    ///     士兵uids
	    /// </summary>
	    private List<int> _soldierUid = new(5);

	    /// <summary>
	    ///     士兵配置
	    /// </summary>
	    private int _soliderConfigId;

        public BarrackLogic(Actor actor) : base(actor) { }

        protected override void setupComponents()
        {
            addComponent(new BuffComp(this, () => BuildingConfig.OwnerBuffs));
            addComponent(new SkillComp(this, () => BuildingConfig.ownerSkills));
            addComponent(new VFXComp(this));
            addComponent(new AttrSimpleProgress(this));
            addComponent(new BeHurtComp(this));
        }

        protected override void setupInput()
        {
            _actorInput = new NoInput(this);
        }

        protected override void onTick(float dt)
        {
            if (BattleManager.CurBattle.RtInfo.CurRoundState != ERoundState.Running)
            {
                return;
            }

            if (_soliderConfigId <= 0)
            {
                return;
            }

            if (_curSoldierCount == _maxSoldierCount)
            {
                return;
            }

            if (_produceInterval < ProduceSoldierTime)
            {
                _produceInterval += dt;
                return;
            }

            _produceInterval = 0;

            ActorManager.Instance.CreateActor(EActorType.Monster, _soliderConfigId, onActorSetup);
        }

        private void onActorSetup(Actor actor)
        {
            ++_curSoldierCount;
            _soldierUid.Add(actor.Uid);
            /*actor.OnModelLoadFinish = (soldier) => {
                soldier.ModelController.Model.transform.position = Actor.Pos;
                soldier.ModelController.Model.transform.rotation = Actor.Rot;
            };*/

            //用CharCtrl挤开士兵
            /*actor.OnInitCallBack = (soldier) => {
                if (soldier.ModelController.Model.TryGetComponent<CharacterController>(out var charCtrl)) {
                    Vector3 randomOffset = new(
                        Random.Range(-2f, 2f),
                        0,
                        Random.Range(-2f, 2f)
                    );
                    charCtrl.Move(randomOffset);

                    soldier.SetAttr(ELogicAttr.AttrPosition, charCtrl.transform.position, false);
                    soldier.SetAttr(ELogicAttr.AttrOriginPos, charCtrl.transform.position, false);
                    soldier.SetAttr(ELogicAttr.AttrRot, Actor.Rot, false);
                }
            };*/

            actor.OnDestroyCallBack = (soldier) =>
            {
                --_curSoldierCount;
                _soldierUid.Remove(soldier.Uid);
            };

            Vector3 randomOffset = new(
                Random.Range(-4f, 4f),
                0,
                Random.Range(-4f, 4f)
            );

            actor.SetAttr(ELogicAttr.AttrPosition, Actor.Pos + randomOffset, false);
            actor.SetAttr(ELogicAttr.AttrOriginPos, Actor.Pos + randomOffset, false);
        }

        public void ChangeSoliderConfigId(int soliderConfigId)
        {
            _soliderConfigId = soliderConfigId;
        }
    }
}