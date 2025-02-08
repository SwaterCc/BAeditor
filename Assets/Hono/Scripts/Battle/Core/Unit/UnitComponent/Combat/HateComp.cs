#region

using System.Collections.Generic;
using Hono.Scripts.Battle.Core;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    /// <summary>
    ///     仇恨对象选择器，选不到默认返回-1
    /// </summary>
    public class HateComp : UnitComponent
    {
        private float _duration;
        private RangeFilterSetting _setting;
        private int _hateUid;
        private bool _isReturnTeam;
        private List<int> _hateUids = new(32);

        public override void Init()
        {
            _setting = new RangeFilterSetting()
            {
                OpenBoxCheck = true,
                BoxData = new CheckBoxData() { ShapeType = ECheckBoxShapeType.Sphere, Radius = 10, },
                // Ranges = new List<FilterCondition>() { new() { conditionType = EFilterConditionType.Faction, value = 2 }, },
                // FilterFunctionType = EFilterFunctionType.Near,
                maxResultCount = 1,
            };
        }

        protected override void onTick(float dt)
        {
            /*if (_duration < 0.5f)
            {
                _duration += dt;
                return;
            }

            if (_hateUid > 0)
            {
                if (!World.Instance.HasActor(_hateUid))
                {
                    _hateUid = -1;
                }

                if (Unit.ActorType == EActorType.Character)
                {
                    var origin = Unit.TargetPos;
                    var dis = Vector3.Distance(origin, Unit.Pos);
                    if (dis > 20.5)
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
                    var origin = Unit.TargetPos;
                    var dis = Vector3.Distance(origin, Unit.Pos);
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

            Unit.SetAttr(EAttrType.AttrHateTargetUid, _hateUid, false);
            _duration = 0;*/
        }

        public void UpdateHateTarget()
        {
            UnitManager.Filter.SearchUnit(Unit, _setting, ref _hateUids);
            if (_hateUids.Count == 0)
            {
                _hateUid = -1;
            }
            else
            {
                _hateUid = _hateUids[0];
            }
        }

        public override void onClear()
        {
            _hateUids.Clear();
            _duration = 0;
            _isReturnTeam = false;
        }
    }
}