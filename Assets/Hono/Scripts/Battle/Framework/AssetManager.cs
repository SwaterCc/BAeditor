using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Hono.Scripts.Battle
{
    public interface IDataHelper { }

    public class DataHelper<T> : IDataHelper where T : ScriptableObject, IAllowedIndexing
    {
        private readonly Dictionary<int, T> _assets = new();
        private readonly Dictionary<int, string> _paths = new Dictionary<int, string>();

        public void AddData(int id, string path, T data)
        {
            if (data == null || !_assets.TryAdd(id, data))
            {
                Debug.LogError($"{typeof(T)} TryAdd {id} id重复");
                return;
            }

            _paths.TryAdd(id, path);
        }

        public bool TryGetPath(int id, out string path)
        {
            return _paths.TryGetValue(id, out path);
        }

        public T Get(int id)
        {
            return _assets[id];
        }

        public void Release(int id)
        {
            if (!_assets.TryGetValue(id, out var asset))
            {
                Debug.LogError($"{id} 数据不存在");
                return;
            }

            _assets.Remove(id);
            Addressables.Release(asset);
        }

        public bool TryGetData(int id, out T data)
        {
            return _assets.TryGetValue(id, out data);
        }
    }

    public interface IReloadHandle
    {
        public void Reload();
    }

    /// <summary>
    /// Demo2使用的数据类，用于加载Asset
    /// </summary>
    public class AssetManager : Tools.Singleton<AssetManager>
    {
        private readonly Dictionary<Type, IDataHelper> _assetDict = new Dictionary<Type, IDataHelper>();
        private readonly List<IReloadHandle> _reloadHandles = new List<IReloadHandle>();
        private bool _isLoadFinish;
        public bool IsLoadFinish => _isLoadFinish;

        public async void Init()
        {
            _isLoadFinish = false;

            List<UniTask> tasks = new List<UniTask>();

            var loadSuccess = addLoadTask<AbilityData>(ref tasks, DataPath.AbilityRoot);
            loadSuccess = loadSuccess && addLoadTask<SkillData>(ref tasks, DataPath.SkillFolder);
            loadSuccess = loadSuccess && addLoadTask<BuffData>(ref tasks, DataPath.BuffFolder);
            //_isLoadFinish = addLoadTask<BulletData>(ref tasks,DataPath.BulletFolder);
            try
            {
                if (loadSuccess)
                {
                    await UniTask.WhenAll(tasks);
                }
                else
                {
                    Debug.LogError("路径收集失败");
                    return;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("资源加载失败");
                return;
            }

            _isLoadFinish = true;
        }

        private bool addLoadTask<T>(ref List<UniTask> tasks, string root) where T : ScriptableObject, IAllowedIndexing
        {
            if (TryGetAllAssetPaths(root, out var paths))
            {
                tasks.Add(loadPathAllAssets<T>(paths));
            }
            else
            {
                Debug.LogError("");
                return false;
            }

            return true;
        }


        private bool TryGetAllAssetPaths(string rootDirectory, out List<string> paths)
        {
            paths = new List<string>();
            // 检查目录是否存在
            if (!Directory.Exists(rootDirectory))
            {
                Debug.LogError("Directory does not exist: " + rootDirectory);
                return false;
            }

            try
            {
                // 获取所有 .asset 文件的路径，包括所有子目录
                string[] assetFiles = Directory.GetFiles(rootDirectory, "*.asset", SearchOption.AllDirectories);
                paths.AddRange(assetFiles);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error accessing directory: " + rootDirectory + "\n" + ex.Message);
                return false;
            }

            return true;
        }

        private async UniTask loadPathAllAssets<T>(List<string> paths) where T : ScriptableObject, IAllowedIndexing
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
                List<UniTask> tasks = new List<UniTask>();

                foreach (var path in paths)
                {
                    tasks.Add(loadAsset<T>(helper, path));
                }

                await UniTask.WhenAll(tasks);

                /*int succeedCount = 0;
                foreach (var task in tasks)
                {
                    if (task.Status == UniTaskStatus.Succeeded)
                    {
                        ++succeedCount;
                    }
                }

                Debug.Log($"加载完成！加载数量 {succeedCount}");*/
            }
            catch (Exception e)
            {
                Debug.LogError($"Unexpected error: {e}");
            }
        }

        public static async UniTask<bool> loadAsset<T>(DataHelper<T> helper, string path)
            where T : ScriptableObject, IAllowedIndexing
        {
            try
            {
                var data = await Addressables.LoadAssetAsync<T>(path).ToUniTask();
                helper.AddData(data.ID, path, data);
            }
            catch (InvalidKeyException)
            {
                Debug.LogWarning($"Asset key not found: {path}");
                return false;
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
                return false;
            }

            return true;
        }

        public void AddReloadHandle(IReloadHandle reloadHandle)
        {
            if (_reloadHandles.Contains(reloadHandle))
            {
                return;
            }
            _reloadHandles.Add(reloadHandle);
        }

        public void CallReloadHandles() {
	        var reloadList = new List<IReloadHandle>(_reloadHandles);
            foreach (var handle in reloadList)
            {
                handle.Reload();
            }
        }

        public void RemoveReloadHandle(IReloadHandle reloadHandle)
        {
            if (!_reloadHandles.Contains(reloadHandle))
            {
                return;
            }
            _reloadHandles.Remove(reloadHandle);
        }
        
        public async UniTask ReloadAsset<T>(int id) where T : ScriptableObject, IAllowedIndexing
        {
#if UNITY_EDITOR
            if (_assetDict.TryGetValue(typeof(T), out var iHelper) && iHelper is DataHelper<T> dataHelper)
            {
                if (dataHelper.TryGetPath(id, out var path))
                {
                    _isLoadFinish = false;
                    dataHelper.Release(id);
                    if (await loadAsset(dataHelper, path))
                    {
                        Debug.Log($"ReloadAssset Type{typeof(T)} id {id} success!");
                        _isLoadFinish = true;
                    }
                }
            }
#endif
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