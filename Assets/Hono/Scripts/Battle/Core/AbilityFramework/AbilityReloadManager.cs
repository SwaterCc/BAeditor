using Hono.Scripts.Battle.AbilityFramework;
using Hono.Scripts.Battle.Core.Base;
using System.Collections.Generic;

namespace Hono.Scripts.Battle.Core.AbilityFramework {
	
	public class AbilityReloadManager : Singleton<AbilityReloadManager> {
		private Dictionary<int, List<Ability>> _reloadCache = new();
		
		
		public void Register(Ability abilityReloadHandle)
		{
			if (!_reloadCache.TryGetValue(abilityReloadHandle.Id, out var list)) {
				list = new List<Ability>(10);
				_reloadCache.Add(abilityReloadHandle.Id, list);
			}
			list.Add(abilityReloadHandle);
		}

		public void CallReloadHandles(int id)
		{
			if (_reloadCache.TryGetValue(id, out var list)) {
				foreach (var abilityReloadHandle in list) {
					abilityReloadHandle.Reload();				}
			}
		}

		public void Unregister(Ability abilityReloadHandle)
		{
			if (_reloadCache.TryGetValue(abilityReloadHandle.Id, out var list)) {
				list.Remove(abilityReloadHandle);
			}
		}
	}
}