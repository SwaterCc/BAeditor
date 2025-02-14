using System;
using UnityEngine;

namespace Hono.Scripts.Battle.Core
{
    [RequireComponent(typeof(PerformanceEffectsPlayer))]
    [RequireComponent(typeof(CharacterController))]
    public class UnitModel : MonoBehaviour
    {
        public Unit Actor { get; private set; }

        /// <summary>
        /// 加载完成时调用
        /// </summary>
        public void OnLoadFinish(ModelControllerComp modelControllerComp)
        {
            Actor = modelControllerComp.Unit;
            //设置子节点的layer
        }
    }
}