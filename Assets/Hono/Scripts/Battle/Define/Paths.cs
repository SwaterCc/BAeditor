#region

using Sirenix.OdinInspector;
using System.Collections.Generic;

#endregion

namespace Hono.Scripts.Battle.Define {
	public class Paths : SerializedScriptableObject {
		public Dictionary<EPathType, List<string>> paths = new();
	}
}