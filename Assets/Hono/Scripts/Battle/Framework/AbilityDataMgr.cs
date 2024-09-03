using System;
using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;
using Hono.Scripts.Battle.Tools;
using Hono.Scripts.Battle.Tools.CustomAttribute;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Hono.Scripts.Battle
{
    public class FuncInfo
    {
        private object[] _paramArr = new object[3];

        public string FuncName;
        public int ParamCount = 3;
        public MethodInfo MethodInfo;
        public bool HasParam;

        public object Invoke(object caller)
        {
            return MethodInfo.Invoke(caller, null);
        }

        public object Invoke(object caller, object[] param)
        {
            if (param.Length != ParamCount)
            {
                Debug.LogError("参数数量不对");
                return null;
            }

            return MethodInfo.Invoke(caller, param);
        }
    }
    
    public class AbilityDataMgr : Singleton<AbilityDataMgr>
    {
        /// <summary>
        /// 函数信息缓存
        /// </summary>
        private readonly Dictionary<string, FuncInfo>
            _cacheMethodInfos = new Dictionary<string, FuncInfo>(64);
        
        /// <summary>
        /// ability数据
        /// </summary>
        private readonly Dictionary<int, AbilityData> _abilityDatas = new(64);

        private AbilityDataList _skill;
        private AbilityDataList _buff;
        private AbilityDataList _bullet;

        private static string RootPath = "Assets/AbilityData/BattleEditorData";
        private static string SkillPath = "Assets/AbilityData/BattleEditorData/Skill";
        private static string BuffPath = "Assets/AbilityData/BattleEditorData/Buff";
        private static string BulletPath = "Assets/AbilityData/BattleEditorData/Bullet";

        private bool IsLoaded = false;

        public async void Init()
        {
            initAbilityFuncCache();
            await loadAbilityDataList();
        }

        private async UniTask loadAbilityDataList()
        {
            try
            {
                _skill = await Addressables.LoadAssetAsync<AbilityDataList>($"{RootPath}/Skills.asset").ToUniTask();
                await loadAbilityDataItem(_skill, SkillPath);
                _buff = await Addressables.LoadAssetAsync<AbilityDataList>($"{RootPath}/Buffs.asset").ToUniTask();
                await loadAbilityDataItem(_buff, BuffPath);
                //tasks.Add(Addressables.LoadAssetAsync<AbilityDataList>($"{RootPath}/Buffs.asset").ToUniTask());
                //tasks.Add(Addressables.LoadAssetAsync<AbilityDataList>($"{RootPath}/Bullets.asset").ToUniTask());
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
          
        }

        private async UniTask loadAbilityDataItem(AbilityDataList list, string path)
        {
            if(list == null) return;
            
            List<UniTask<AbilityData>> tasks = new List<UniTask<AbilityData>>();
            foreach (var skillItem in list.Items)
            {
                tasks.Add(Addressables.LoadAssetAsync<AbilityData>($"{path}/{skillItem.Value.configId}.asset")
                    .ToUniTask());
            }

            var abilityDatas = await UniTask.WhenAll(tasks);
            for (var index = 0; index < abilityDatas.Length; index++)
            {
                var ability = abilityDatas[index];
                if (ability != null)
                {
                    _abilityDatas.Add(ability.ConfigId, ability);
                }
            }
        }

        private void initAbilityFuncCache()
        {
            _cacheMethodInfos.Clear();
            Type type = typeof(AbilityCacheFuncDefine);

            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);

            foreach (var method in methods)
            {
                AbilityFuncCache attr = null;
                foreach (var obj in method.GetCustomAttributes(typeof(AbilityFuncCache), false))
                {
                    if (obj is AbilityFuncCache cache)
                    {
                        attr = cache;
                    }
                }

                if (attr == null) continue;

                if (!_cacheMethodInfos.ContainsKey(method.Name))
                {
                    var info = new FuncInfo
                    {
                        FuncName = method.Name,
                        ParamCount = method.GetParameters().Length,
                        MethodInfo = method,
                        HasParam = method.GetParameters().Length > 0,
                    };

                    _cacheMethodInfos.Add(method.Name, info);
                }
            }
        }
        
        /// <summary>
        /// 获取函数缓存
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public FuncInfo GetFuncInfo(string func)
        {
            //TODO:字符串有消耗
            return _cacheMethodInfos[func];
        }
        
        /// <summary>
        /// 获取指定Id的data
        /// </summary>
        /// <param name="dataId"></param>
        /// <returns></returns>
        public AbilityData GetAbilityData(int dataId)
        {
            //临时用直接加载
            return _abilityDatas[dataId];
        }

        public bool TryGetAbilityData(int id,out AbilityData data)
        {
            if (_abilityDatas.TryGetValue(id, out data))
            {
                return true;
            }

            return false;
        }
    }
}