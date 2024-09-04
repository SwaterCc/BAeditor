using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class ActorManager
    {
        private Filter _filter;

        public List<int> UseFilter(FilterSetting setting)
        {
            if (setting == null)
            {
                Debug.LogError("Setting is Empty!");
                return null;
            }
            _filter.SettingChange(setting);
            return _filter.GetResults();
        }

        public class Filter
        {
            private FilterSetting _filterSetting;
            private ActorManager _actorManager;

            public Filter(ActorManager actorManager)
            {
                _actorManager = actorManager;
            }

            public void SettingChange(FilterSetting setting)
            {
                _filterSetting = setting;
            }

            private bool rangeCheck(ActorLogic logic, FilterRange range)
            {
                switch (range.RangeType)
                {
                    case EFilterRangeType.Tag:
                        return logic.HasTag(range.Value);
                    case EFilterRangeType.ActorState:
                        return logic.CurState() == (EActorState)range.Value;
                    case EFilterRangeType.AbilityID:
                        return logic.CurState() == (EActorState)range.Value;
                }

                Debug.LogError($"使用了未实现的范围筛选 settingId {_filterSetting.id} type {range.RangeType}");
                return false;
            }

            private bool getCompareRes(ECompareResType compareResType, int flag)
            {
                switch (compareResType)
                {
                    case ECompareResType.Less:
                        return flag < 0;
                    case ECompareResType.LessAndEqual:
                        return flag <= 0;
                    case ECompareResType.Equal:
                        return flag == 0;
                    case ECompareResType.More:
                        return flag > 0;
                    case ECompareResType.MoreAndEqual:
                        return flag >= 0;
                }

                return true;
            }

            public List<int> GetResults()
            {
                if (_filterSetting == null)
                {
                    Debug.LogError("筛选器设置为空");
                    return null;
                }

                List<int> actorIds = new List<int>();

                foreach (var actor in _actorManager._runningActorList)
                {
                    bool pass = true;
                    foreach (var range in _filterSetting.Ranges)
                    {
                        if (!rangeCheck(actor.ActorLogic, range))
                        {
                            pass = false;
                            break;
                        }
                    }

                    if (!pass)
                    {
                        continue;
                    }

                    foreach (var compare in _filterSetting.Compares)
                    {
                        var left = (IComparable)actor.ActorLogic.GetAttrBox(compare.AttrType);
                        int res = left.CompareTo(compare.CompareValue);
                        if (!getCompareRes(compare.CompareResType, res))
                        {
                            pass = false;
                            break;
                        }
                    }

                    if (!pass)
                    {
                        continue;
                    }

                    actorIds.Add(actor.Uid);
                }

                return actorIds;
            }
        }
    }
}