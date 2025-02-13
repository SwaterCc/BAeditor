#region

using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Core;
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
        //特效组件的唯一Id生成器
        private static readonly CommonUtility.IdGenerator VFXIdGenerator = CommonUtility.GetIdGenerator();

        private readonly Dictionary<int, VFXInfo> _vfxes = new(32);
        public Dictionary<int, VFXInfo> VFXDict => _vfxes;

        private readonly List<VFXInfo> _removeList = new(32);

        public event Action<VFXInfo> VFXAdd;
        
        public event Action<VFXInfo> VFXRemove;

        public VFXComp(ComponentCtorParams ctorParams) : base(ctorParams) { }
        
        public override void Init() { }

        /// <summary>
        /// 添加特效
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public int AddVFXByKey(VFXSetting setting)
        {
            
            var vfxInfo = new VFXInfo(VFXIdGenerator.GenerateId(),"", setting);

            switch (setting.vfxBindType)
            {
                case EVFXType.InWorld:
                    vfxInfo.Pos = Unit.UnitTransform.Pos + Unit.UnitTransform.YAxisAngle * (Vector3)setting.offset;
                    vfxInfo.Rot = Quaternion.AngleAxis(Unit.UnitTransform.YAxisAngle,Vector3.up) * Quaternion.Euler(setting.rot);
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
                addInfoToList(vfxInfo);
            }
            else
            {
                World.Current.WorldRoot.VFXComp.addInfoToList(vfxInfo);
            }

            return vfxInfo.Uid;
        }

        public void RemoveVFX(int key)
        {
            if (_vfxes.TryGetValue(key, out var obj))
            {
                onRemove(obj);
            }
        }
        
        private void addInfoToList(VFXInfo vfxObj)
        {
            _vfxes.Add(vfxObj.Uid, vfxObj);
            VFXAdd?.Invoke(vfxObj);
        }
        
        private void onRemove(VFXInfo obj)
        {
            _vfxes.Remove(obj.Uid);
            VFXRemove?.Invoke(obj);
        }

        protected override void onTick(float dt)
        {
            /*for (var index = 0; index < _vfxes.Count; index++)
            {
                VFXInfo obj = _vfxes[index];
                if (obj.IsExpired)
                {
                    _removeList.Add(obj.Value);
                }

                if (obj.Value.Setting.vfxBindType == EVFXType.FollowActor)
                {
                    obj.Value.Pos = Unit.UnitTransform.Pos + (Vector3)obj.Value.Setting.offset;
                }

                obj.Value.OnTick(dt);
            }*/

            foreach (var obj in _removeList)
            {
                onRemove(obj);
            }

            _removeList.Clear();
        }

        protected override void onClear()
        {
            _removeList.Clear();
            foreach (var obj in _vfxes)
            {
                onRemove(obj.Value);
            }

            _vfxes.Clear();
            VFXAdd = null;
            VFXRemove = null;
        }
    }
}