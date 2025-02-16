using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// 美术层必要组件，该组件会作为PEPlayer的识别标准，播放动画
    /// </summary>
    public class ActorModelHandler : MonoBehaviour
    {
        public Animator animator;
        public Dictionary<string, Transform> points = new();
        
        [Button("收集组件和挂点")]
        public void Bind()
        {
            animator = GetComponent<Animator>();
            foreach (var child in transform)
            {
                if (child is Transform childTransform)
                {
                    if (childTransform.name == "_point")
                    {
                        points.Add(childTransform.name, childTransform);
                    }
                }
            }
        }

        public void PlayAnim(string key)
        {
            animator?.Play(key);
        }

        public Transform GetPoint(string key)
        {
            return points.GetValueOrDefault(key, null);
        }
        
        public bool TryGetPoint(string key,out Transform point)
        {
            return points.TryGetValue(key, out point);
        }
    }
}