using System.Collections.Generic;
using Hono.Scripts.Battle.Tools;

namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        /// <summary>
        /// Ability可执行函数的调用接口
        /// </summary>
        public partial class AFuncInvoker : Singleton<AFuncInvoker>
        {
            /// <summary>
            /// 函数列表
            /// </summary>
            private readonly Dictionary<string, AFunctionWrap> _functionWraps = new(100);

            /// <summary>
            /// 注册函数
            /// </summary>
            /// <param name="funcName"></param>
            /// <param name="wrap"></param>
            private void addWrap(string funcName, AFunctionWrap wrap)
            {
                _functionWraps.TryAdd(funcName, wrap);
            }

            /// <summary>
            /// 函數包
            /// </summary>
            /// <param name="funcName"></param>
            /// <returns></returns>
            public AFunctionWrap Get(string funcName)
            {
                return _functionWraps[funcName];
            }
        }
    }
}