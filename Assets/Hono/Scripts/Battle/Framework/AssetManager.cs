using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Hono.Scripts.Battle.Tools;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Hono.Scripts.Battle
{
    public interface IDataHelper { }

    public class DataHelper<T> : IDataHelper where T : ScriptableObject, IAllowedIndexing
    {
        private Dictionary<int, T> _datas = new();

        public void AddData(int id, T data)
        {
            if (data == null || !_datas.TryAdd(id, data))
            {
                Debug.LogError($"{typeof(T)} TryAdd {id} id重复");
            }
        }

        public T Get(int id)
        {
            return _datas[id];
        }

        public bool TryGetData(int id, out T data)
        {
            return _datas.TryGetValue(id, out data);
        }
    }


    /// <summary>
    /// Demo2使用的数据类，用于加载Asset
    /// </summary>
    public class AssetManager : Tools.Singleton<AssetManager>
    {
        private readonly Dictionary<Type, IDataHelper> _assetDict = new Dictionary<Type, IDataHelper>();

        private bool _isLoadFinish;
        public bool IsLoadFinish => _isLoadFinish;

        private readonly HashSet<string> _loadLabels = new()
        {
            "ability",
            "skill",
            "buff",
            "bullet",
            "actorPrototype",
            "actorLogic",
            "actorShow",
            "hitBoxData"
        };

        private AssetLabelCounter _assetLabelCounter;

        public async void Init()
        {
            _isLoadFinish = false;

            await loadLabelCount();

            List<UniTask> tasks = new List<UniTask>();
            //加载数据
            tasks.Add(register<AbilityData>("ability"));
            tasks.Add(register<SkillData>("skill"));
            tasks.Add(register<BuffData>("buff"));
            //tasks.Add(register<BulletData>("bullet"));
            
            await UniTask.WhenAll(tasks);
            _isLoadFinish = true;
        }


        private async UniTask loadLabelCount()
        {
            _assetLabelCounter = await Addressables
                .LoadAssetAsync<AssetLabelCounter>("Assets/BattleData/LabelCount.asset").ToUniTask();
        }

        private async UniTask register<T>(string label) where T : ScriptableObject, IAllowedIndexing
        {
            if (!_assetLabelCounter.LabelCounts.TryGetValue(label, out var count) || count == 0)
            {
                return;
            }

            var key = typeof(T);
            if (_assetDict.ContainsKey(key))
            {
                Debug.LogError($"{key} 该类型的资源重复加载");
                return;
            }

            var helper = new DataHelper<T>();
            _assetDict.Add(key, helper);

            try
            {
                var datas = await Addressables.LoadAssetsAsync<T>(label, null).ToUniTask();

                if (datas == null)
                {
                    Debug.Log($"Asset key empty");
                    return;
                }

                foreach (var data in datas)
                {
                    helper.AddData(data.ID, data);
                }

                Debug.Log($"Asset key {label} 加载完成！加载数量 {datas.Count}");
            }
            catch (InvalidKeyException)
            {
                Debug.LogWarning($"Asset key not found: {label}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Unexpected error: {e}");
            }
        }

        public async void ReloadAsset<T>(int id, string path) where T : ScriptableObject, IAllowedIndexing { }

        public T GetData<T>(int id) where T : ScriptableObject, IAllowedIndexing
        {
            if (_assetDict.TryGetValue(typeof(T), out var idataHelper) && idataHelper is DataHelper<T> dataHelper)
            {
                if (dataHelper.TryGetData(id, out var row))
                {
                    return row;
                }
            }

            return null;
        }

        public bool TryGetData<T>(int id, out T data) where T : ScriptableObject, IAllowedIndexing
        {
            data = null;
            if (_assetDict.TryGetValue(typeof(T), out var idataHelper) && idataHelper is DataHelper<T> dataHelper)
            {
                if (dataHelper.TryGetData(id, out data))
                {
                    return true;
                }
            }

            return false;
        }
    }
}