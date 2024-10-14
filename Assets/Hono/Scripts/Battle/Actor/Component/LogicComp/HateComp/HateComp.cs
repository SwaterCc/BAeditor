using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class ActorLogic
    {
        public abstract class HateSelection
        {
            public abstract int GetHateTargetUid();
        }
        
        /// <summary>
        /// 仇恨对象选择器，选不到默认返回-1
        /// </summary>
        public class HateComp : ALogicComponent
        {
            private HateSelection _hateSelection;
            private float _duration;
            private FilterSetting _setting;
            public HateComp(ActorLogic logic, HateSelection hateSelection) : base(logic)
            {
                _hateSelection = hateSelection;
            }
            
            public override void Init()
            {
                _setting = new FilterSetting()
                {
                    OpenBoxCheck = true,
                    BoxData = new CheckBoxData()
                    {
                        ShapeType = ECheckBoxShapeType.Sphere,
                        Radius = 10,
                    },
                    Ranges = new List<FilterRange>()
                    {
                        new() { RangeType = EFilterRangeType.Faction, Value = 2 },
                    },
                    FilterFunctionType = EFilterFunctionType.Near,
                    MaxTargetCount = 1,
                };
            }

            protected override void onTick(float dt)
            {
                if (_duration > 0.5f)
                {
                    UpdateHateTarget();
                    _duration = 0;
                }

                _duration += dt;
            }

            public void UpdateHateTarget()
            {
                var hateUid = ActorManager.Instance.UseFilter(Actor, _setting);
                if (hateUid.Count == 0)
                {
                    Actor.SetAttr(ELogicAttr.AttrHateTargetUid, -1, false);
                    return;
                }

                if (Actor.ActorType == EActorType.Pawn)
                {
                    var origin = Actor.GetAttr<Vector3>(ELogicAttr.AttrOriginPos);
                    var dis = Vector3.Distance(origin, Actor.Pos);
                    if (dis > 20)
                    {
                        Actor.SetAttr(ELogicAttr.AttrHateTargetUid, -1, false);
                        return;
                    }
                }
                Actor.SetAttr(ELogicAttr.AttrHateTargetUid, hateUid, false);
            }
        }
    }
}