using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hono.Scripts.Battle.Scene
{
    public class BattleLevelData : ScriptableObject
    {
        [LabelText("失败后可重复挑战")]
        public bool CanRepeat;
        
        [LabelText("波次信息")]
        public List<RoundData> RoundDatas;
    }
}