#region

using Unity.Mathematics;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    public struct VFXInfo
    {
        /// <summary>
        /// 特效Uid,全局共享同一个Id生成器
        /// </summary>
        public readonly int Uid;

        /// <summary>
        /// 特效设置
        /// </summary>
        public VFXSetting Setting { get; }

        /// <summary>
        /// VFX的实际路径
        /// </summary>
        public string Path;

        /// <summary>
        /// 是否过期
        /// </summary>
        public bool IsExpired { get; private set; }

        /// <summary>
        /// 坐标
        /// </summary>
        public Vector3 Pos { get;  set; }

        /// <summary>
        /// 旋转
        /// </summary>
        public Quaternion Rot { get;  set; }

        /// <summary>
        /// 缩放
        /// </summary>
        public Vector3 Scale { get;  set; }

        /// <summary>
        /// 持续时间
        /// </summary>
        private float _duration;

        public VFXInfo(int uid, string path, VFXSetting setting)
        {
            Uid = uid;
            Setting = setting;
            IsExpired = false;
            Pos = Vector3.zero;
            Rot = quaternion.identity;
            Scale = setting.scale * Vector3.one;
            _duration = 0;
            Path = path;
        }

        public void OnTick(float dt)
        {
            if (Setting.duration > 0 && Setting.duration < _duration)
            {
                IsExpired = true;
            }

            _duration += dt;
        }
    }
}