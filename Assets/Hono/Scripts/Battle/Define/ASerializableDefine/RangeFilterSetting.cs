#region

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

#endregion

namespace Hono.Scripts.Battle
{
    [Serializable]
    public struct AttrCompare
    {
        public EAttrType attrType;
        public ECompareResType compareResType;
        public float compareValue;
    }

    [Serializable]
    public struct FilterCondition
    {
        public EFilterConditionType conditionType;
        public bool isReverse;
        public int value;
    }

    [Serializable]
    public class RangeFilterSetting
    {
        [BoxGroup("范围设置", true, true)]
        public bool OpenBoxCheck = true;

        [BoxGroup("范围设置", true, true)]
        public CheckBoxData BoxData = new();
        
        [BoxGroup("范围设置", true, true)]
        public int maxResultCount = -1;

        [BoxGroup("范围设置", true, true)]
        public bool includeSelf;

        [BoxGroup("筛选条件设置", true, true)]
        public ConditionFilterSetting conditionFilterSetting;
        
        [BoxGroup("范围设置", true, true)]
        public EFilterFunctionType filterFunctionType;
    }

    [Serializable]
    public class ConditionFilterSetting
    {
        [BoxGroup("筛选条件设置", true, true)]
        public List<FilterCondition> conditions = new();

        [BoxGroup("筛选条件设置", true, true)]
        public List<AttrCompare> attrCompares = new();
    }
}