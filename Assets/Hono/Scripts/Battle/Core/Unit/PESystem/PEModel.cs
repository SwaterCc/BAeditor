using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// 美术层必要组件，该组件会作为PEPlayer的识别标准，播放动画
    /// </summary>
    public class PEModelHandler : MonoBehaviour
    {
        public Animator Animator;
        public Dictionary<string, Transform> Point = new();
        public string pointName;
        public void Bind()
        {
            Animator = GetComponent<Animator>();
            
        }

        public void PlayAnim(string key)
        {
            Animator?.Play(key);
        }

        public Transform GetPoint(string key)
        {
            return Point.GetValueOrDefault(key, null);
        }
        
        public bool TryGetPoint(string key,out Transform point)
        {
            return Point.TryGetValue(key, out point);
        }
    }
}