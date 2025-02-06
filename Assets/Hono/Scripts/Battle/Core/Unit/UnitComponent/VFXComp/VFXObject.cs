#region

using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    public class VFXObject : ICPoolObject
    {
        /// <summary>
        /// 特效Uid,全局共享同一个Id生成器
        /// </summary>
        public int Uid { get; private set; }

        /// <summary>
        /// 特效设置
        /// </summary>
        public VFXSetting Setting { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsExpired { get; private set; }

        /// <summary>
        /// 坐标
        /// </summary>
        public Vector3 Pos;

        /// <summary>
        /// 旋转
        /// </summary>
        public Quaternion Rot;

        /// <summary>
        /// 缩放
        /// </summary>
        public Vector3 Scale;

        /// <summary>
        /// 持续时间
        /// </summary>
        private float _duration;

        public void OnRent(in int uid, in VFXSetting setting)
        {
            Uid = uid;
            Setting = setting;
            IsExpired = false;
            Scale = setting.Scale * Vector3.one;
        }

        public void OnTick(float dt)
        {
            if (Setting.Duration > 0 && Setting.Duration < _duration)
            {
                IsExpired = true;
            }

            _duration += dt;
        }

        public void OnRecycle()
        {
            Uid = 0;
            _duration = 0;
            IsExpired = false;
            Pos = Vector3.zero;
            Rot = Quaternion.identity;
            Scale = Vector3.one;
            Setting = null;
        }
    }
}