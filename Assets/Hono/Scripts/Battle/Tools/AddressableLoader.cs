using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Hono.Scripts.Battle.Tools
{
    public class AddressableLoader
    {
        public async void LoadScriptableObjects<T>(List<string> keys, System.Action<List<T>> onFinish) where T : ScriptableObject
        {
            List<UniTask<T>> tasks = new List<UniTask<T>>();
        
            // 创建并启动所有任务
            foreach (var key in keys)
            {
                var task = LoadAssetAsyncWithHandling<T>(key);
                tasks.Add(task);
            }

            // 并行等待所有任务完成
            var results = await UniTask.WhenAll(tasks);

            // 处理结果
            List<T> successfulResults = new List<T>();
            for (var i = 0; i < results.Length; i++)
            {
                if (results[i] != null)
                {
                    successfulResults.Add(results[i]);
                }
                else
                {
                    Debug.LogError($"Failed to load {typeof(T).Name} with key: {keys[i]}");
                }
            }

            onFinish.Invoke(successfulResults);
        }

        private async UniTask<T> LoadAssetAsyncWithHandling<T>(string key) where T : ScriptableObject
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(key);
            try
            {
                return await handle.ToUniTask();
            }
            catch
            {
                return null;
            }
            finally
            {
                Addressables.Release(handle);
            }
        }
    }
}