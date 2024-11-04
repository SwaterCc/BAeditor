#region

using System.Collections.Generic;

#endregion

namespace Hono.Scripts.Battle
{
    public class DamageFuncInfo
    {
        public List<int> ConditionIds = new List<int>();
        public List<List<int>> ConditionParams = new List<List<int>>();

        public string ValueFuncName = "normal";

        public List<int> ValueParams = new List<int>();
    }
}