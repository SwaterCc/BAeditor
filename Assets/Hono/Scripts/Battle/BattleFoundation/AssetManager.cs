#region

using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Hono.Scripts.Battle.Core.Base;
using Hono.Scripts.Battle.Tools;
using UnityEngine;
using UnityEngine.AddressableAssets;

#endregion

namespace Hono.Scripts.Battle
{
    public interface IReloadHandle
    {
        public void Reload();
    }

    /// <summary>
    ///     Demo2使用的数据类，用于加载Asset
    /// </summary>
    public class AssetManager : BattleFoundation<AssetManager>
    {
        private readonly Dictionary<Type, IDataHelper> _assetCache = new();
        private readonly List<IReloadHandle> _reloadHandles = new();

        public override async UniTask AsyncLoad()
        {
            var assets = await Addressables.LoadAssetsAsync<ASerializableData>("aSerializableData").ToUniTask();
            foreach (var asset in assets)
            {
                switch (asset)
                {
                    case AbilityData data:
                    {
                        if (!_assetCache.TryGetValue(typeof(AbilityData), out var dataHelper))
                        {
                            dataHelper = new DataHelper<AbilityData>();
                            _assetCache.Add(typeof(AbilityData), dataHelper);
                        }

                        ((DataHelper<AbilityData>)dataHelper).AddData(data.id, data.path, data);
                        break;
                    }
                    case SkillData data:
                    {
                        if (!_assetCache.TryGetValue(typeof(SkillData), out var dataHelper))
                        {
                            dataHelper = new DataHelper<SkillData>();
                            _assetCache.Add(typeof(SkillData), dataHelper);
                        }

                        ((DataHelper<SkillData>)dataHelper).AddData(data.id, data.path, data);
                        break;
                    }
                    case BuffData data:
                    {
                        if (!_assetCache.TryGetValue(typeof(BuffData), out var dataHelper))
                        {
                            dataHelper = new DataHelper<BuffData>();
                            _assetCache.Add(typeof(BuffData), dataHelper);
                        }

                        ((DataHelper<BuffData>)dataHelper).AddData(data.id, data.path, data);
                        break;
                    }
                    case BulletData data:
                    {
                        if (!_assetCache.TryGetValue(typeof(BulletData), out var dataHelper))
                        {
                            dataHelper = new DataHelper<BulletData>();
                            _assetCache.Add(typeof(BulletData), dataHelper);
                        }

                        ((DataHelper<BulletData>)dataHelper).AddData(data.id, data.path, data);
                        break;
                    }
                }
            }
        }

        public void AddReloadHandle(IReloadHandle reloadHandle)
        {
            if (_reloadHandles.Contains(reloadHandle))
            {
                return;
            }

            _reloadHandles.Add(reloadHandle);
        }

        public void CallReloadHandles()
        {
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

        public async UniTask ReloadAsset<T>(int id) where T : ASerializableData
        {
#if UNITY_EDITOR
            if (_assetCache.TryGetValue(typeof(T), out var iHelper) && iHelper is DataHelper<T> dataHelper)
            {
                if (dataHelper.TryGetPath(id, out var path))
                {
                    dataHelper.Release(id);
                    /*if (await loadAsset(dataHelper, path))
                    {
                        Debug.Log($"ReloadAssset Type{typeof(T)} id {id} success!");
                        _isLoadFinish = true;
                    }*/
                }
            }
#endif
        }

        public T GetData<T>(int id) where T : ASerializableData
        {
            if (_assetCache.TryGetValue(typeof(T), out var idataHelper) && idataHelper is DataHelper<T> dataHelper)
            {
                if (dataHelper.TryGetData(id, out var row))
                {
                    return row;
                }
            }

            return null;
        }

        public bool TryGetData<T>(int id, out T data) where T : ASerializableData
        {
            data = null;
            if (_assetCache.TryGetValue(typeof(T), out var idataHelper) && idataHelper is DataHelper<T> dataHelper)
            {
                if (dataHelper.TryGetData(id, out data))
                {
                    return true;
                }
            }

            return false;
        }
    }

    public interface IDataHelper { }

    public class DataHelper<T> : IDataHelper where T : ASerializableData
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
}