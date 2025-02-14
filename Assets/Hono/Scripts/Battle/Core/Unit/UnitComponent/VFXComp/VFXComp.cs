#region

using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Core;
using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Tools;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// 视觉特效逻辑层记录
    /// </summary>
    public class VFXComp : UnitComponent
    {
        public Dictionary<int, VFXInfo> VFXLookup => _caches;
        public event Action<VFXInfo> VFXAdd;
        public event Action<VFXInfo> VFXRemove;

        //特效组件的唯一Id生成器
        private static readonly CommonUtility.IdGenerator VFXIdGenerator = CommonUtility.GetIdGenerator();
        private readonly Dictionary<int, VFXInfo> _caches = new(32);
        private readonly List<VFXInfo> _removeList = new(32);
        

        public override void Init() { }

        /// <summary>
        /// 使用资源路径添加VFX
        /// </summary>
        /// <returns></returns>
        //private int addVFXByResPath(string path, VFXSetting setting) { }

        /// <summary>
        /// 添加特效
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public int AddVFXByKey(VFXSetting setting)
        {
            var vfxInfo = new VFXInfo(VFXIdGenerator.GenerateId(), "", setting);

            switch (setting.vfxBindType)
            {
                case EVFXType.InWorld:
                    vfxInfo.Pos = Unit.UnitTransform.Pos + Unit.UnitTransform.Rot * setting.offset;
                    vfxInfo.Rot = Unit.UnitTransform.Rot * Quaternion.Euler(setting.rot);
                    break;
                case EVFXType.FollowActor:
                    vfxInfo.Pos = Unit.UnitTransform.Pos + setting.offset;
                    vfxInfo.Rot = Quaternion.Euler(setting.rot);
                    break;
                case EVFXType.BindActorBone:
                    vfxInfo.Pos = setting.offset;
                    vfxInfo.Rot = Quaternion.Euler(setting.rot);
                    break;
            }

            if (setting.vfxBindType != EVFXType.InWorld)
            {
                addVFX(vfxInfo);
            }
            else
            {
                //World.Current.WorldRoot.VFXComp.addVFX(vfxInfo);
            }

            return vfxInfo.Uid;
        }

        public void RemoveVFX(int key)
        {
            if (_caches.TryGetValue(key, out var obj))
            {
                removeVFX(obj);
            }
        }

        private void addVFX(VFXInfo vfxObj)
        {
            VFXAdd?.Invoke(vfxObj);
            _caches.Add(vfxObj.Uid, vfxObj);
        }

        private void removeVFX(VFXInfo obj)
        {
            VFXRemove?.Invoke(obj);
            _caches.Remove(obj.Uid);
        }

        protected override void onTick(float dt)
        {
            for (var index = 0; index < _caches.Count; index++)
            {
                VFXInfo obj = _caches[index];
                if (obj.IsExpired)
                {
                    _removeList.Add(obj);
                }

                if (obj.Setting.vfxBindType == EVFXType.FollowActor)
                {
                    obj.Pos = Unit.UnitTransform.Pos + obj.Setting.offset;
                }

                obj.OnTick(dt);
            }

            foreach (var obj in _removeList)
            {
                removeVFX(obj);
            }

            _removeList.Clear();
        }

        protected override void onClear()
        {
            _removeList.Clear();
            foreach (var obj in _caches)
            {
                removeVFX(obj.Value);
            }

            _caches.Clear();
            VFXAdd = null;
            VFXRemove = null;
        }
    }
}