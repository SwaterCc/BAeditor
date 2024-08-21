using System;
using System.Collections.Generic;
using AbilityRes;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Battle.Tools
{
    public class AbilityDataCacheMgr
    {
        private static AbilityDataCacheMgr _instance;

        public static AbilityDataCacheMgr Instance
        {
            get { return _instance ??= new AbilityDataCacheMgr(); }
        }

        private Dictionary<int, AbilityData> _abilityDatas = new Dictionary<int, AbilityData>(64);

        private AbilityDataList _skill;
        private AbilityDataList _buff;
        private AbilityDataList _bullet;

        public static string RootPath = "Assets/AbilityData/BattleEditorData";
        public static string SkillPath = "Assets/AbilityData/BattleEditorData/Skill";
        public static string BuffPath = "Assets/AbilityData/BattleEditorData/Buff";
        public static string BulletPath = "Assets/AbilityData/BattleEditorData/Bullet";
        
        public static bool IsLoaded = false;
        public async void Init()
        {
            await LoadAbilityDataList();
            await LoadAbilityDataItem(_skill,SkillPath);
        }
        
        private async UniTask LoadAbilityDataList()
        {
            try
            {
                _skill = await Addressables.LoadAssetAsync<AbilityDataList>($"{RootPath}/Skills.asset");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            //tasks.Add(Addressables.LoadAssetAsync<AbilityDataList>($"{RootPath}/Buffs.asset").ToUniTask());
            //tasks.Add(Addressables.LoadAssetAsync<AbilityDataList>($"{RootPath}/Bullets.asset").ToUniTask());
        }

        private async UniTask LoadAbilityDataItem(AbilityDataList list, string path)
        {
            List<UniTask<AbilityData>> tasks = new List<UniTask<AbilityData>>();
            foreach (var skillItem in _skill.Items)
            {
                tasks.Add(Addressables.LoadAssetAsync<AbilityData>($"{path}/{skillItem.Value.configId}.asset").ToUniTask());
            }

            var abilityDatas = await UniTask.WhenAll(tasks);
            foreach (var ability in abilityDatas)
            {
                _abilityDatas.Add(ability.ConfigId,ability);
            }
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
    }
}