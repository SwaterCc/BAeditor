﻿using System;
using System.Collections.Generic;
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
    
    
    public class FilterSetting : ScriptableObject,IAllowedIndexing
    {
        public int ID => id;
        public int id;

        public List<FilterRange> Ranges = new List<FilterRange>();
        public List<FilterAttrCompare> Compares = new List<FilterAttrCompare>();
    }
}