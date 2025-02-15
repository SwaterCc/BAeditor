#region

using System.Collections.Generic;

#endregion

namespace Hono.Scripts.Battle
{
    public class DamageFuncInfo : IGPoolObject
    {
        public List<int> ConditionIds;
        public List<List<int>> ConditionParams;
        public string ValueFuncName = "normal";
        public List<int> ValueParams;

        public void OnRecycle()
        {
            ConditionIds = null;
            ConditionParams = null;
            ValueParams = null;
            ValueFuncName = "normal";
        }
    }
}