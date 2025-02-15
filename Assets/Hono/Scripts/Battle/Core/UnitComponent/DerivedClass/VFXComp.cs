#region

using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Core;
using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Tools;
using UnityEngine;
using XLua;

#endregion

namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// 视觉特效逻辑层记录
    /// </summary>
    public class VFXComp : UnitComponent, IGPoolObject
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
        public int AddVFXByResPath(string path, VFXSetting setting)
        {
            var vfxInfo = new VFXInfo(VFXIdGenerator.GenerateId(), path, setting);

            //计算特效坐标
            if (setting.isWorldVFX)
            {
                Vector3 rootPos = Vector3.zero;
                Quaternion rootRot = Quaternion.identity;
                if (!setting.isWorldPoint)
                {
                    rootPos = Unit.UnitTransform.Pos;
                    rootRot = Unit.UnitTransform.Rot;
                }

                vfxInfo.Pos = rootPos + Unit.UnitTransform.Rot * setting.offset;
                vfxInfo.Rot = rootRot * Quaternion.Euler(setting.rotOffset);
                addVFX(vfxInfo);
            }
            else
            {
                vfxInfo.Pos = setting.offset;
                vfxInfo.Rot = Quaternion.Euler(setting.rotOffset);
                World.Current.WorldRoot.VFXComp.addVFX(vfxInfo);
            }

            return vfxInfo.Uid;
        }

        /// <summary>
        /// 使用PE模板添加特效
        /// </summary>
        /// <param name="key"></param>
        /// <param name="setting"></param>
        /// <returns></returns>
        public int AddVFXByKey(string key, VFXSetting setting)
        {
            int tplId = Unit.GetAttr(EAttrType.AttrResReplTplOverrideId) == 0
                ? Unit.GetAttr(EAttrType.AttrResReplTplBaseId)
                : Unit.GetAttr(EAttrType.AttrResReplTplOverrideId);

            var path = ResReplTplDB.Instance.GetVFXPath(tplId, key);
            
            if (string.IsNullOrEmpty(path))
            {
                return -1;
            }
            
            var vfxInfo = new VFXInfo(VFXIdGenerator.GenerateId(), path, setting);

            //计算特效坐标
            if (setting.isWorldVFX)
            {
                Vector3 rootPos = Vector3.zero;
                Quaternion rootRot = Quaternion.identity;
                if (!setting.isWorldPoint)
                {
                    rootPos = Unit.UnitTransform.Pos;
                    rootRot = Unit.UnitTransform.Rot;
                }

                vfxInfo.Pos = rootPos + Unit.UnitTransform.Rot * setting.offset;
                vfxInfo.Rot = rootRot * Quaternion.Euler(setting.rotOffset);
                addVFX(vfxInfo);
            }
            else
            {
                vfxInfo.Pos = setting.offset;
                vfxInfo.Rot = Quaternion.Euler(setting.rotOffset);
                World.Current.WorldRoot.VFXComp.addVFX(vfxInfo);
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

        private void addVFX(VFXInfo vfxInfo)
        {
            _caches.Add(vfxInfo.Uid, vfxInfo);
            if (UnityAdapter.Instance.TryGetUnityObjectProxy(Unit.Uid, out var proxy))
            {
                proxy.AddVFX(vfxInfo);
            }
        }

        private void removeVFX(VFXInfo vfxInfo)
        {
            _caches.Remove(vfxInfo.Uid);
            if (UnityAdapter.Instance.TryGetUnityObjectProxy(Unit.Uid, out var proxy))
            {
                proxy.RemoveVFX(vfxInfo);
            }
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

                obj.OnTick(dt);
            }

            foreach (var obj in _removeList)
            {
                removeVFX(obj);
            }

            _removeList.Clear();
        }

        public override void Recycle()
        {
            GPool<VFXComp>.Pool.Recycle(this);
        }

        public void OnRecycle()
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