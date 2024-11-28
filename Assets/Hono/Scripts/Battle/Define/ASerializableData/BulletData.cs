#region

using UnityEngine;
using UnityEngine.Serialization;

#endregion

namespace Hono.Scripts.Battle
{
    public class BulletSetting
    {
        public Vector3 Offset;
        public float Angle;
        public float Speed;
    }

    public class BulletData : ASerializableData
    {
        /// <summary>
        /// 使用Ability自定义位移类型
        /// </summary>
        public bool CustomMotion;

        /// <summary>
        /// 位移类型
        /// </summary>
        public EMotionType MotionType = EMotionType.Liner;

        /// <summary>
        /// 关闭跟随，默认为跟随目标
        /// </summary>
        public bool CloseFollowTarget;

        /// <summary>
        /// 速度，废弃，移动到setting中
        /// </summary>
        public float BulletSpeed;

        /// <summary>
        /// 是否命中路径中的Actor
        /// </summary>
        public bool IsHitPathActor;

        /// <summary>
        /// 伤害id
        /// </summary>
        public int DamageConfigId;

        public float BulletLifeTime;

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