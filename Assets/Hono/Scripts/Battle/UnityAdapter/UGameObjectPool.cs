using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Hono.Scripts.Battle.Core.Base;
using Hono.Scripts.Battle.Tools;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Hono.Scripts.Battle
{
    /// <summary>
    /// U池，全称UnityGameObjectPool，用于管理GameObject
    /// </summary>
    public class UGameObjectPool : MonoSingleton<UGameObjectPool>
    {
        /// <summary>
        /// 池
        /// </summary>
        private readonly Dictionary<string, Queue<GameObject>> _gameObjectCache = new(50);
        
        public async UniTask<GameObject> Get(string path, CancellationTokenSource cancelSource)
        {
            return await Get(path, null, Vector3.zero, Vector3.one, Quaternion.identity, true, cancelSource);
        }

        public async UniTask<GameObject> Get(string path, Vector3 position, CancellationTokenSource cancelSource)
        {
            return await Get(path, null, position, Vector3.one, Quaternion.identity, true, cancelSource);
        }

        public async UniTask<GameObject> Get(string path, Transform parent, Vector3 position, CancellationTokenSource cancelSource)
        {
            return await Get(path, parent, position, Vector3.one, Quaternion.identity, false, cancelSource);
        }

        public async UniTask<GameObject> Get(string path, Transform parent, Vector3 position, Quaternion rotation, CancellationTokenSource cancelSource)
        {
            return await Get(path, parent, position, Vector3.one, rotation, false, cancelSource);
        }

        /// <summary>
        /// 获取UnityGameObject
        /// </summary>
        /// <param name="path">Addressable路径</param>
        /// <param name="parent">父对象</param>
        /// <param name="position">坐标</param>
        /// <param name="scale">缩放</param>
        /// <param name="rotation">旋转</param>
        /// <param name="worldSpace">是否为世界空间</param>
        /// <param name="cancelSource"></param>
        /// <returns></returns>
        public async UniTask<GameObject> Get(string path,
            Transform parent,
            Vector3 position,
            Vector3 scale,
            Quaternion rotation,
            bool worldSpace,
            CancellationTokenSource cancelSource)
        {
            GameObject result = null;

            if (!_gameObjectCache.TryGetValue(path, out var objectPool))
            {
                //找不到对应的key值，先确定路径能加载出对象
                try
                {
                    result = await Addressables.LoadAssetAsync<GameObject>(path).ToUniTask(cancellationToken: cancelSource.Token);
                    //有对象，路径正确
                    objectPool = new Queue<GameObject>(10);
                    _gameObjectCache.Add(path, objectPool);
                }
                catch (OperationCanceledException)
                {
                    return result;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    return result;
                }
            }

            if (objectPool.Count == 0)
            {
                //池为空，加载新对象
                try
                {
                    result = await Addressables.LoadAssetAsync<GameObject>(path).ToUniTask(cancellationToken: cancelSource.Token);
                }
                catch (OperationCanceledException)
                {
                    return null;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    return null;
                }
            }
            else
            {
                result = objectPool.Dequeue();
            }

            result.transform.SetParent(parent);
            if (worldSpace)
            {
                result.transform.position = position;
                result.transform.rotation = rotation;
                result.transform.localScale = scale;
            }
            else
            {
                result.transform.localScale = position;
                result.transform.localRotation = rotation;
                result.transform.localScale = scale;
            }

            result.SetActive(true);
            return result;
        }

        public bool Recycle(string path, GameObject obj)
        {
            if (obj == null)
            {
                return false;
            }
            
            if (!_gameObjectCache.ContainsKey(path))
            {
                Debug.LogError("回池路径错误！");
                return false;
            }

            obj.SetActive(false);
            obj.transform.parent = transform;
            _gameObjectCache[path].Enqueue(obj);
            return true;
        }
    }
}