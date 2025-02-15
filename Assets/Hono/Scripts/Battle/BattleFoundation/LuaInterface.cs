#region

using System;
using System.IO;
using Cysharp.Threading.Tasks;
using Hono.Scripts.Battle.Core;
using Hono.Scripts.Battle.Core.Base;
using Hono.Scripts.Battle.Tools;
using UnityEngine;
using UnityEngine.AddressableAssets;
using XLua;

#endregion

namespace Hono.Scripts.Battle
{
    public class LuaInterface : Singleton<LuaInterface>, IBattleFrameworkAsyncInit
    {
        private static LuaEnv _luaEnv;
        private static LuaFunction _damageProcessMain;
        private static LuaFunction _factionMain;

        public async UniTask AsyncInit()
        {
            _luaEnv = new LuaEnv();

            try
            {
                var luaScripts = await Addressables.LoadAssetsAsync<TextAsset>("luaScript", null).ToUniTask();

                foreach (var luaFile in luaScripts)
                {
                    Debug.Log($"load lua {luaFile.name}");
                    _luaEnv.DoString(luaFile.text);
                }

                loadDamageFunc();
                loadFactionFunc();

                Debug.Log("LuaInterface Init Finish！");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private void Reload()
        {
            string luaScriptsPath = Application.dataPath + "/Hono/Scripts/Battle/LuaScript";
            // 获取 LuaScripts 目录下的所有 Lua 脚本文件 TODO:临时做法，后续要改
            string[] luaFiles = Directory.GetFiles(luaScriptsPath, "*.lua");

            foreach (string luaFile in luaFiles)
            {
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
            //_factionMain = _luaEnv.Global.GetInPath<LuaFunction>("Faction.GetFaction");
        }
        
        public void CalcDamageResults(Unit attacker, Unit target, DamagePipLine data)
        {
            //TODO：临时做法，会有性能开销，后续导出
            _damageProcessMain.Call(attacker, target, data);
        }

        public static int GetFaction(int factionId1, int factionId2)
        {
            return Instance.getFaction(factionId1, factionId2);
        }

        private int getFaction(int factionId1, int factionId2)
        {
            var rets = _factionMain.Call(factionId1, factionId2);
            if (rets is { Length: > 0 })
            {
                return (int)((long)rets[0]);
            }
            else
            {
                Debug.LogError("lua函数返回失败");
            }

            return 0;
        }

        public void Dispose()
        {
            _luaEnv.Dispose();
        }
    }
}