﻿using System;
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
        public int Value;
    }
    
    [Serializable]
    public class FilterSetting
    {
        public bool OpenBoxCheck;
        [ShowIf("OpenBoxCheck")]
        public CheckBoxData BoxData = new();
        public int MaxTargetCount = -1;
        public List<FilterRange> Ranges = new List<FilterRange>();
        public List<FilterAttrCompare> Compares = new List<FilterAttrCompare>();
    }
}