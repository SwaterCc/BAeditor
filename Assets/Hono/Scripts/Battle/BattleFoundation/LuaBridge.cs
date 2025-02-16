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
    public class LuaBridge : BattleFoundation<LuaBridge>
    {
        private static LuaEnv _luaEnv;
        private static LuaFunction _damageProcessMain;
        private static LuaFunction _factionMain;

        public override async UniTask AsyncLoad()
        {
            _luaEnv = new LuaEnv();
            _luaEnv.AddLoader(CustomMyLoader);
            try
            {
                await UniTask.RunOnThreadPool((() => _luaEnv.DoString("require 'luaMain'")));
                loadDamageFunc();
                Debug.Log("LuaInterface Init Finish！");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        
        private byte[] CustomMyLoader(ref string fileName)
        {
            byte[] byArrayReturn = null; //返回数据
            //定义lua路径
            string luaPath = Application.dataPath + "/Hono/Scripts/Battle/LuaScript/" + fileName + ".lua";
            //读取lua路径中指定lua文件内容
            string strLuaContent = File.ReadAllText(luaPath);
            //数据类型转换
            byArrayReturn = System.Text.Encoding.UTF8.GetBytes(strLuaContent);
            return byArrayReturn;
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
        }

        private static void loadDamageFunc()
        {
            _damageProcessMain = _luaEnv.Global.GetInPath<LuaFunction>("DamageProcess.DamageProcessMain");
        }
        
        
        public void CalcDamageResults(Unit attacker, Unit target, DamagePipLine data)
        {
            //TODO：临时做法，会有性能开销，后续导出
            _damageProcessMain.Call(attacker, target, data);
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