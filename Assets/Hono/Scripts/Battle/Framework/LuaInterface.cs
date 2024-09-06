using System.IO;
using UnityEngine;
using XLua;

namespace Hono.Scripts.Battle
{
    public static class LuaInterface
    {
        private static LuaEnv _luaEnv;
        private static LuaFunction _damageProcessMain;
        private static LuaFunction _factionMain;
        public static void Init() {
            _luaEnv = new LuaEnv();
            //调用lua
            string luaScriptsPath = Application.dataPath + "/Hono/Scripts/Battle/LuaScript";
            // 获取 LuaScripts 目录下的所有 Lua 脚本文件 TODO:临时做法，后续要改
            string[] luaFiles = Directory.GetFiles(luaScriptsPath, "*.lua");

            foreach (string luaFile in luaFiles) {
                string luaScript = File.ReadAllText(luaFile);
                _luaEnv.DoString(luaScript);
            }
            loadDamageFunc();
            loadFactionFunc();
        }

        private static void Reload()
        {
            string luaScriptsPath = Application.dataPath + "/Hono/Scripts/Battle/LuaScript";
            // 获取 LuaScripts 目录下的所有 Lua 脚本文件 TODO:临时做法，后续要改
            string[] luaFiles = Directory.GetFiles(luaScriptsPath, "*.lua");

            foreach (string luaFile in luaFiles) {
                string luaScript = File.ReadAllText(luaFile);
                _luaEnv.DoString(luaScript);
            }
            loadDamageFunc();
            loadFactionFunc();
        }
        
        private static void loadDamageFunc()
        {
            _damageProcessMain = _luaEnv.Global.GetInPath<LuaFunction>("DamageProcess.DamageProcessMain");
        }

        private static void loadFactionFunc()
        {
            _factionMain = _luaEnv.Global.GetInPath<LuaFunction>("Faction.GetFaction");
        }
        
        public static DamageResults GetDamageResults(ActorLogic attacker, ActorLogic target, DamageInfo damageInfo,
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

        public static int GetFaction(int factionId1,int factionId2)
        {
            Reload();
            var rets = _factionMain.Call(factionId1, factionId2);
            if (rets is { Length: > 0 }) {
                return (int)((long)rets[0]);
            }
            else {
                Debug.LogError("lua函数返回失败");
            }
            
            return 0;
        }
        
        public static void Dispose() {
            _luaEnv.Dispose();
        }
    }
}