using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Hono.Scripts.Battle.Tools
{
    [CreateAssetMenu(menuName = "战斗编辑器/AssetLabelCounter")] 
    public class AssetLabelCounter : SerializedScriptableObject
    {
        public Dictionary<string, int> LabelCounts = new();
    }
}