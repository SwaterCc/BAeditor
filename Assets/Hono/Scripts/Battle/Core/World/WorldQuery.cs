using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Tools;
using UnityEngine;
using UnityEngine.Profiling;

namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// Unit检索器，用于快速搜索符合条件的Unit
    /// </summary>
    public class WorldQuery
    {
        private readonly WorldInstance _worldInstance;
        /// <summary>
        /// Unit索引字典
        /// </summary>
        private readonly Dictionary<int, Unit> _lookup = new(2000);
        private readonly List<Unit> _filterActors = new(32);
        private List<int> _checkBoxResult = new(32);
        private RangeFilterSetting _rangeFilterSetting;
        private Unit _filterUser;
        private Vector3 _centerPos;
        private float _yAxisAngle;

        public WorldQuery(WorldInstance worldInstance)
        {
            _worldInstance = worldInstance;
        }

        public Unit GetUnit(int uid)
        {
            return _lookup.GetValueOrDefault(uid, null);
        }

        public bool TryGetUnit(int uid, out Unit unit)
        {
            return _lookup.TryGetValue(uid, out unit);
        }

        public bool ContainsUnit(int unitUid)
        {
            return _lookup.ContainsKey(unitUid);
        }

        public void AddUnitLookup(Unit unit)
        {
            _lookup.Add(unit.Uid, unit);
        }

        public bool TryAddUnitLookup(Unit unit)
        {
            return _lookup.TryAdd(unit.Uid, unit);
        }

        public void RemoveUnitLookup(Unit unit)
        {
            _lookup.Remove(unit.Uid);
        }

        /// <summary>
        /// 搜索符合条件的Unit
        /// </summary>
        /// <param name="user"></param>
        /// <param name="centerPos">搜索区域中心坐标</param>
        /// <param name="yAxisAngle">搜索区域朝向</param>
        /// <param name="setting"></param>
        /// <param name="result"></param>
        public void SearchUnits(Unit user,
            Vector3 centerPos,
            float yAxisAngle,
            RangeFilterSetting setting,
            ref List<int> result)
        {
            if (setting == null)
            {
                Debug.LogError("Setting is Empty!");
                return;
            }

            if (result == null)
            {
                Debug.LogError("result list is Empty!");
                return;
            }

            _filterUser = user;
            _centerPos = centerPos;
            _yAxisAngle = yAxisAngle;
            _rangeFilterSetting = setting;
            result.Clear();
            
            getResults(ref result);
            
            _filterUser = null;
            _centerPos = Vector3.zero;
            _yAxisAngle = 0;
            _rangeFilterSetting = null;
        }

        public bool ConditionFilter(Unit filterUser, Unit checkUnit, ConditionFilterSetting setting)
        {
            if (checkUnit == null)
            {
                return false;
            }

            _filterUser = filterUser;
            bool result = checkActorPass(checkUnit, setting);
            _filterUser = null;
            return result;
        }
        
        public bool ConditionFilter(Unit filterUser, int checkActorUid, ConditionFilterSetting setting)
        {
            if (!_lookup.TryGetValue(checkActorUid, out var unit))
            {
                return false;
            }

            _filterUser = filterUser;
            bool result = checkActorPass(unit, setting);
            _filterUser = null;
            return result;
        }

        private bool rangeCheck(Unit unit, in FilterCondition condition)
        {
            bool checkResult = false;
            switch (condition.conditionType)
            {
                case EFilterConditionType.ActorType:
                    if (unit is not Actor actor)
                        return false;
                    checkResult = (int)actor.ActorType == condition.value;
                    break;
                case EFilterConditionType.Tag:
                    checkResult = unit.Tags.HasTag(condition.value);
                    break;
                case EFilterConditionType.Faction:
                    checkResult =
                        Faction.IsSpecialRelationship(_filterUser, unit, (EFactionRelationship)condition.value);
                    break;
                default:
                    Debug.LogError($"使用了未实现的范围筛选 settingId {_filterUser.Uid} type {condition.conditionType}");
                    return false;
            }

            if (condition.isReverse)
                checkResult = !checkResult;

            return checkResult;
        }

        private bool getCompareRes(in ECompareResType compareResType, int flag)
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

        private bool checkActorPass(Unit unit, ConditionFilterSetting conditionFilterSetting)
        {
            foreach (var condition in conditionFilterSetting.conditions)
            {
                if (!rangeCheck(unit, condition))
                {
                    return false;
                }
            }

            foreach (var compare in conditionFilterSetting.attrCompares)
            {
                var left = unit.GetAttr(compare.attrType);
                int res = left.CompareTo((int)compare.compareValue);
                if (!getCompareRes(compare.compareResType, res))
                {
                    return false;
                }
            }

            return true;
        }

        private void getResults(ref List<int> result)
        {
            Profiler.BeginSample("UseFilter");
            if (_rangeFilterSetting == null)
            {
                Debug.LogError("筛选器设置为空");
                return;
            }

            if (_rangeFilterSetting.OpenBoxCheck)
            {
                var rot = _filterUser.UnitTransform.Rot;
                
                if (CommonUtility.HitRayCast(_rangeFilterSetting.BoxData, _centerPos, rot, ref _checkBoxResult))
                {
                    foreach (var uid in _checkBoxResult)
                    {
                        var unSelectable =
                            _lookup[uid].GetAttr(EAttrType.AttrUnselectable) != 0;
                        if (unSelectable) continue;
                        _filterActors.Add(_lookup[uid]);
                    }

                    _checkBoxResult.Clear();
                }
            }
            else
            {
                _filterActors.AddRange(_lookup.Values);
            }

            if (!_rangeFilterSetting.includeSelf && _filterActors.Contains(_filterUser))
            {
                _filterActors.Remove(_filterUser);
            }

            foreach (var actor in _filterActors)
            {
                if (checkActorPass(actor, _rangeFilterSetting.conditionFilterSetting))
                    result.Add(actor.Uid);
            }

            if (_rangeFilterSetting.maxResultCount <= 0)
            {
                return;
            }

            if (_rangeFilterSetting.maxResultCount >= result.Count)
            {
                return;
            }

            //TODO:实现比较器，减少GC
            switch (_rangeFilterSetting.filterFunctionType)
            {
                case EFilterFunctionType.LeastHp:
                    result.Sort((aUid, bUid) =>
                    {
                        int aHp = _lookup[aUid].GetAttr(EAttrType.AttrHp);
                        int bHp = _lookup[bUid].GetAttr(EAttrType.AttrHp);
                        return aHp.CompareTo(bHp);
                    });
                    break;
                case EFilterFunctionType.HighestHp:
                    result.Sort((aUid, bUid) =>
                    {
                        int aHp = _lookup[aUid].GetAttr(EAttrType.AttrHp);
                        int bHp = _lookup[bUid].GetAttr(EAttrType.AttrHp);
                        return aHp.CompareTo(bHp) * -1;
                    });
                    break;
                case EFilterFunctionType.Far:
                    result.Sort((aUid, bUid) =>
                    {
                        var aPos = _lookup[aUid].UnitTransform.Pos;
                        var bPos = _lookup[bUid].UnitTransform.Pos;
                        var selfPos = _filterUser.UnitTransform.Pos;

                        var aDis = Math.Abs(Vector3.Distance(aPos, selfPos));
                        var bDis = Math.Abs(Vector3.Distance(bPos, selfPos));
                        return aDis.CompareTo(bDis) * -1;
                    });
                    break;
                case EFilterFunctionType.Near:
                    result.Sort((aUid, bUid) =>
                    {
                        var aPos = _lookup[aUid].UnitTransform.Pos;
                        var bPos = _lookup[bUid].UnitTransform.Pos;
                        var selfPos = _filterUser.UnitTransform.Pos;

                        var aDis = Math.Abs(Vector3.Distance(aPos, selfPos));
                        var bDis = Math.Abs(Vector3.Distance(bPos, selfPos));
                        return aDis.CompareTo(bDis);
                    });
                    break;
                case EFilterFunctionType.Random:
                    CommonUtility.Shuffle(ref result);
                    break;
                case EFilterFunctionType.ControlPlayer:
                    break;
            }

            for (int index = result.Count - 1; index > -1; index--)
            {
                if (index > _rangeFilterSetting.maxResultCount - 1)
                {
                    result.RemoveAt(index);
                }
            }

            _filterActors.Clear();
            _checkBoxResult.Clear();

            Profiler.EndSample();
        }
    }
}