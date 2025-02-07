using System;
using UnityEngine;

namespace Hono.Scripts.Battle.Core
{
    [RequireComponent(typeof(PerformanceEffectsPlayer))]
    [RequireComponent(typeof(CharacterController))]
    public class ActorModel : MonoBehaviour
    {
        public Actor Actor { get; private set; }

        /// <summary>
        /// 加载完成时调用
        /// </summary>
        public void OnLoadFinish(ActorModelController modelController)
        {
            Actor = modelController.Self;
            //设置子节点的layer
        }
    }
}