using System.Collections.Generic;
using Hono.Scripts.Battle.Event;
using Unity.VisualScripting;
using UnityEngine;

namespace Hono.Scripts.Battle.Core
{
    //仅Actor对象存在父子关系
    //其实是两种打击系统，瞬时打击和延时打击,子弹是打击点的包装，继承？
    public interface IHitCheck
    {
        /// <summary>
        /// 打击盒数据
        /// </summary>
        public HitBoxData HitBoxData { get; private set; }
        
        public virtual void Init(HitBoxData hitBoxData)
        {
            HitBoxData = hitBoxData;
        }
    }
}