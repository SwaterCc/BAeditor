using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hono.Scripts.Battle.Scene
{
    public class BattleLevelData : SerializedScriptableObject
    {
        [LabelText("失败后可重复挑战")]
        public bool CanRepeat;
        
        [LabelText("波次信息")]
        [ListDrawerSettings(ShowFoldout = true)]
        public List<RoundData> RoundDatas = new();
    }
}