#region

using System.Collections.Generic;
using Sirenix.OdinInspector;

#endregion

namespace Hono.Scripts.Battle.Define
{
    public class Paths : SerializedScriptableObject
    {
        public Dictionary<EPathType, List<string>> paths = new();
    }
}