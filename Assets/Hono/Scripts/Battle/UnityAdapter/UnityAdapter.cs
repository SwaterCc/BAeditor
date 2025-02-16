using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Hono.Scripts.Battle.Core;
using Hono.Scripts.Battle.Core.Base;

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
        ///  创建代理
        /// </summary>
        /// <param name="unit"></param>
        public async UniTask CreateUnityObjectProxy(Unit unit)
        {
            UnityObjectProxy proxy = null;
            proxy = _proxyPool.Count == 0 ? new UnityObjectProxy() : _proxyPool.Dequeue();
            proxy.BindUnit(unit);
            await proxy.LoadProxy();
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
            return _runningProxies.TryGetValue(uid, out var proxy) ? proxy : null;
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