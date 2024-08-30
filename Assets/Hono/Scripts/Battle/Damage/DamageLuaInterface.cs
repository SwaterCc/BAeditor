using System.IO;
using UnityEngine;
using XLua;

namespace Hono.Scripts.Battle {
	public class DamageLuaInterface {
		private LuaEnv _damageLuaEnv;
		private LuaFunction _damageProcessMain;

		public void Init() {
			_damageLuaEnv = new LuaEnv();
			//调用lua
			string luaScriptsPath = Application.dataPath + "/Hono/Scripts/Battle/LuaScript/Damage";

			// 获取 LuaScripts 目录下的所有 Lua 脚本文件 TODO:临时做法，后续要改
			string[] luaFiles = Directory.GetFiles(luaScriptsPath, "*.lua");

			foreach (string luaFile in luaFiles) {
				string luaScript = File.ReadAllText(luaFile);
				_damageLuaEnv.DoString(luaScript);
			}

			_damageProcessMain = _damageLuaEnv.Global.GetInPath<LuaFunction>("DamageProcess.DamageProcessMain");
		}

		public DamageResults GetDamageResults(ActorLogic attacker, ActorLogic target, DamageInfo damageInfo,
			DamageItem config) {
			//TODO：临时做法，会有性能开销，后续导出
			var rets = _damageProcessMain.Call(attacker, target, damageInfo, config, typeof(DamageResults));
			if (rets is { Length: > 0 } && rets[0] is DamageResults results) {
				return results;
			}
			else {
				Debug.LogError("lua函数返回失败");
			}

			return null;
		}

		public void Dispose() {
			_damageLuaEnv.Dispose();
		}
	}
}