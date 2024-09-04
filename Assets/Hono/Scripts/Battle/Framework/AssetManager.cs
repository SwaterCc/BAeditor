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

        private readonly Dictionary<ECheckBoxShapeType, CheckBoxHandle> _checkBoxDict =
            new Dictionary<ECheckBoxShapeType, CheckBoxHandle>();

        private bool _isLoadFinish;
        public bool IsLoadFinish => _isLoadFinish;

        public async void Init()
        {
            _isLoadFinish = false;
            List<UniTask> tasks = new List<UniTask>();
            //加载数据
            tasks.Add(register<AbilityData>("ability"));
            tasks.Add(register<SkillData>("skill"));
            tasks.Add(register<BuffData>("buff"));
            //tasks.Add(register<BulletData>("bullet"));
            tasks.Add(register<ActorPrototypeData>("actorPrototype"));
            tasks.Add(register<ActorLogicData>("actorLogic"));
            tasks.Add(register<ActorShowData>("actorShow"));
            tasks.Add(register<HitBoxData>("hitBoxData"));
            tasks.Add(register<FilterSetting>("filterSetting"));

            //加载检测盒子
            tasks.Add(loadCheckBoxes("checkBox"));
            await UniTask.WhenAll(tasks);
            _isLoadFinish = true;
        }

        private async UniTask register<T>(string label) where T : ScriptableObject, IAllowedIndexing
        {
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
                var datas = await Addressables.LoadAssetsAsync<T>(label, (data) =>
                {
                    if (data == null) Debug.LogError($"{label}某资源加载失败！");
                }).ToUniTask();

                foreach (var data in datas)
                {
                    helper.AddData(data.ID, data);
                }

                Debug.Log($"asset key {label} 加载完成！加载数量 {datas.Count}");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }

        private async UniTask loadCheckBoxes(string label)
        {
            try
            {
                var datas = await Addressables.LoadAssetsAsync<GameObject>(label, (data) =>
                {
                    if (data == null) Debug.LogError($"{label}某资源加载失败！");
                }).ToUniTask();

                foreach (var gameObject in datas)
                {
                    if (!gameObject || !gameObject.TryGetComponent<CheckBoxHandle>(out var handle))
                        return;
                    _checkBoxDict.Add(handle.eCheckBoxShapeType, handle);
                }

                Debug.Log($"asset key {label} 加载完成！加载数量 {datas.Count}");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }

        public async void ReloadAsset<T>(int id, string path) where T : ScriptableObject, IAllowedIndexing
        {
            
        }

        public CheckBoxHandle GetCheckBox(ECheckBoxShapeType boxShapeType)
        {
            return _checkBoxDict[boxShapeType];
        }

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
    }
}