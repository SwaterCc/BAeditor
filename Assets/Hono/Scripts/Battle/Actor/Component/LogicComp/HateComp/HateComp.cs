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
            private int _hateUid;
            private bool _isReturnTeam;
            
            public HateComp(ActorLogic logic) : base(logic)
            {
               
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
                if (_duration < 0.5f)
                {
                    _duration += dt;
                    return;
                }

                if (_hateUid > 0)
                {
                    var rtState = ActorManager.Instance.GetActorRtState(_hateUid);
                    if (rtState != EActorRunningState.Active)
                    {
                        _hateUid = -1;
                    }
                    
                    if (Actor.ActorType == EActorType.Pawn)
                    {
                        var origin = Actor.GetAttr<Vector3>(ELogicAttr.AttrOriginPos);
                        var dis = Vector3.Distance(origin, Actor.Pos);
                        if (dis > 10.5)
                        {
                            _hateUid = -1;
                            _isReturnTeam = true;
                        }
                    }
                }
                
                if (_hateUid <= 0)
                {
                    if (_isReturnTeam)
                    {
                        var origin = Actor.GetAttr<Vector3>(ELogicAttr.AttrOriginPos);
                        var dis = Vector3.Distance(origin, Actor.Pos);
                        if (dis < 1f)
                        {
                            _isReturnTeam = false;
                            UpdateHateTarget();
                        }
                    }
                    else
                    {
                        UpdateHateTarget();
                    }
                }
                
                Actor.SetAttr(ELogicAttr.AttrHateTargetUid, _hateUid, false);
                _duration = 0;
            }

            public void UpdateHateTarget()
            {
                var hateUids = ActorManager.Instance.UseFilter(Actor, _setting);
                if (hateUids.Count == 0)
                {
                    _hateUid = -1;
                }
                else
                {
                    _hateUid = hateUids[0];
                }
            }
        }
    }
}