using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public class STagTree : ScriptableObject
    {
        public TagTreeItem root = new();
    }
}