using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Tools;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    [Serializable]
    public struct FilterAttrCompare
    {
        public ELogicAttr AttrType;
        public ECompareResType CompareResType;
        public float CompareValue;
    }

    [Serializable]
    public struct FilterRange
    {
        public EFilterRangeType RangeType;
        public bool IsReverse; 
        public int Value;
    }
    
    [Serializable]
    public class FilterSetting
    {
        [HideInInspector]
        [SerializeField]
        private bool _isOnlyCondition;
        [HideIfGroup("_isOnlyCondition")]
        [BoxGroup("_isOnlyCondition/范围设置",true,true)]
        public bool OpenBoxCheck = true;
        [HideIfGroup("_isOnlyCondition")]
        [BoxGroup("_isOnlyCondition/范围设置",true,true)]
        public CheckBoxData BoxData = new();
        [HideIfGroup("_isOnlyCondition")]
        [BoxGroup("_isOnlyCondition/范围设置",true,true)]
        public int MaxTargetCount = -1;
        [HideIfGroup("_isOnlyCondition")]
        [BoxGroup("_isOnlyCondition/范围设置",true,true)]
        public bool HasSelf;
        [BoxGroup("筛选条件设置",true,true)]
        public List<FilterRange> Ranges = new List<FilterRange>();
        [BoxGroup("筛选条件设置",true,true)]
        public List<FilterAttrCompare> Compares = new List<FilterAttrCompare>();
        [HideIfGroup("_isOnlyCondition")]
        [BoxGroup("_isOnlyCondition/范围设置",true,true)]
        public EFilterFunctionType FilterFunctionType;

        public FilterSetting(bool isOnlyCondition = false)
        {
            _isOnlyCondition = isOnlyCondition;
        }
    }

    public static class FilterSettingTmp {
	    public static FilterSetting SelfSetting = new FilterSetting() {
		    MaxTargetCount = 1,
		    HasSelf = true,
		    BoxData = new CheckBoxData() { ShapeType = ECheckBoxShapeType.Sphere, Radius = 0.3f, },
	    };
    }
}