using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Hono.Scripts.Battle.Core;
using Hono.Scripts.Battle.Core.Base;
using UnityEngine.Profiling;

namespace Hono.Scripts.Battle
{
    /// <summary>
    /// Unity适配器
    /// </summary>
    public class UnityAdapter : Singleton<UnityAdapter>
    {
        /// <summary>
        /// 激活中的代理
        /// </summary>
        private readonly Dictionary<int, UnityObjectProxy> _runningProxies = new(1000);

        /// <summary>
        /// 代理池子
        /// </summary>
        private readonly Queue<UnityObjectProxy> _proxyPool = new(1000);
        
        /// <summary>
        /// 同步Transform，同时依靠unity的物理修正坐标
        /// </summary>
        public void SyncProxiesTransform() {
	        foreach (var proxy in _runningProxies.Values) {
		        proxy.SyncTransform();
	        }
        }
        
        /// <summary>
        ///  创建代理
        /// </summary>
        /// <param name="unit"></param>
        public async UniTask CreateUnityObjectProxy(Unit unit)
        {
            UnityObjectProxy proxy = null;
            proxy = _proxyPool.Count == 0 ? new UnityObjectProxy() : _proxyPool.Dequeue();
            proxy.BindUnit(unit);
            await proxy.LoadProxy();
            _runningProxies.Add(unit.Uid, proxy);
        }

        /// <summary>
        /// 删除代理
        /// </summary>
        public void RemoveUnitObjectProxy(int uid)
        {
            if (!_runningProxies.Remove(uid, out var proxy))
            {
                return;
            }
            proxy.OnRecycle();
            _proxyPool.Enqueue(proxy);
        }

        /// <summary>
        /// 获取代理
        /// </summary>
        /// <returns></returns>
        public UnityObjectProxy GetUnityObjectProxy(int uid)
        {
            return _runningProxies.GetValueOrDefault(uid);
        }

        /// <summary>
        /// 获取代理
        /// </summary>
        /// <returns></returns>
        public bool TryGetUnityObjectProxy(int uid, out UnityObjectProxy proxy)
        {
            return _runningProxies.TryGetValue(uid, out proxy);
        }
    }
}