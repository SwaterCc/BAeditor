using System;
using UnityEngine;

namespace Hono.Scripts.Battle.Core
{
    [RequireComponent(typeof(PerformanceEffectsPlayer))]
    [RequireComponent(typeof(CharacterController))]
    public class ActorModel : MonoBehaviour
    {
        /// <summary>
        /// 加载完成时调用
        /// </summary>
        public void OnLoadFinish(ActorModelController modelController) { }
    }
}