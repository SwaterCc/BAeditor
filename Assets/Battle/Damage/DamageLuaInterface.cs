using System.IO;
using UnityEngine;
using XLua;

namespace Hono.Scripts.Battle {
	public static class DamageLuaInterface {
		public static DamageResults GetDamageResults(ActorLogic attacker, ActorLogic target, DamageInfo damageInfo) {
			//调用lua
			string luaScriptsPath = Application.dataPath + "/Hono/Scripts/Battle/LuaScripts/Damage";

			// 获取 LuaScripts 目录下的所有 Lua 脚本文件
			string[] luaFiles = Directory.GetFiles(luaScriptsPath, "*.lua");

			var luaEnv = LuaClient.Instance.GetEnv();
			foreach (string luaFile in luaFiles)
			{
				string luaScript = File.ReadAllText(luaFile);
				luaEnv.DoString(luaScript);
			}
			
			LuaFunction func1 = luaEnv.Global.Get<LuaFunction>("CalculateDamage");
			var objs = func1.Call();
			
			return new DamageResults();
		}
	}
}