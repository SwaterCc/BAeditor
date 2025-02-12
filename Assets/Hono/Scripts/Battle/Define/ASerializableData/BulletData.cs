#region

using UnityEngine;
using UnityEngine.Serialization;

#endregion

namespace Hono.Scripts.Battle
{
    public class BulletData : ASerializableData
    {
        /// <summary>
        /// 使用Ability自定义位移类型
        /// </summary>
        public bool CustomMotion;

        /// <summary>
        /// 位移类型
        /// </summary>
        public EBulletMotionType bulletMotionType = EBulletMotionType.Liner;

        /// <summary>
        /// 关闭跟随，默认为跟随目标
        /// </summary>
        public bool FollowTarget;
        
        /// <summary>
        /// 是否命中路径中的Actor
        /// </summary>
        public bool IsHitPathActor;
        
        /// <summary>
        /// 子弹生命时长
        /// </summary>
        public float BulletLifeTime;

        /// <summary>
        /// 最小命中间隔
        /// </summary>
        public float MinHitInterval;
        
        /// <summary>
        /// 最大命中次数
        /// </summary>
        public int MaxHitCount;

        /// <summary>
        /// 飞行时特效
        /// </summary>
        public string FlyVFX;

        /// <summary>
        /// 命中时特效
        /// </summary>
        public string HitVFX;
    }
}