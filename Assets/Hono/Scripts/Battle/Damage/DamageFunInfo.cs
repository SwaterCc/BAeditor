using System.Collections.Generic;

namespace Hono.Scripts.Battle {
	public class DamageFuncInfo {
		public List<int> ConditionIds = new List<int>();
		public List<List<int>> ConditionParams = new List<List<int>>();
		public string ValueFuncName;
		public List<int> ValueParams = new List<int>();
	}
}