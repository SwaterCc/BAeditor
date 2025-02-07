#region

using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Core;
using Hono.Scripts.Battle.Tools;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    /// <summary>
    ///     视觉特效
    /// </summary>
    public class VFXComp : UnitComponent
    {
        //特效组件的唯一Id生成器
        private static readonly CommonUtility.IdGenerator IDGenerator = CommonUtility.GetIdGenerator();

        private readonly Dictionary<int, VFXInfo> _vfxes = new(32);
        public Dictionary<int, VFXInfo> VFXDict => _vfxes;

        private readonly List<VFXInfo> _removeList = new(32);

        public Action<VFXInfo> VFXAdd;
        public Action<VFXInfo> VFXRemove;

        public override void Init() { }

        public int AddVFXObject(string vfxKey, VFXSetting setting)
        {
            var vfxObj = GPool<VFXInfo>.Pool.Rent();
            vfxObj.OnRent(IDGenerator.GenerateId(), setting);

            switch (setting.VFXBindType)
            {
                case EVFXType.InWorld:
                    vfxObj.Pos = Unit.UnitTransform.Pos + Unit.UnitTransform.YAxisAngle * setting.Offset;
                    vfxObj.Rot = Quaternion.AngleAxis(Unit.UnitTransform.YAxisAngle,Vector3.up) * Quaternion.Euler(setting.Rot);
                    break;
                case EVFXType.FollowActor:
                    vfxObj.Pos = Unit.UnitTransform.Pos + (Vector3)setting.Offset;
                    vfxObj.Rot = Quaternion.Euler(setting.Rot);
                    break;
                case EVFXType.BindActorBone:
                    vfxObj.Pos = setting.Offset;
                    vfxObj.Rot = Quaternion.Euler(setting.Rot);
                    break;
            }

            if (setting.VFXBindType != EVFXType.InWorld)
            {
                AddVFXToList(vfxObj);
            }
            else
            {
                //World.VFXComp.AddVFXToList(vfxObj);
            }

            return vfxObj.Uid;
        }

        protected void AddVFXToList(VFXInfo vfxObj)
        {
            _vfxes.Add(vfxObj.Uid, vfxObj);
            VFXAdd?.Invoke(vfxObj);
        }

        public void RemoveVFX(int key)
        {
            if (_vfxes.TryGetValue(key, out var obj))
            {
                onRemove(obj);
            }
        }

        protected void onRemove(VFXInfo obj)
        {
            _vfxes.Remove(obj.Uid);
            GPool<VFXInfo>.Pool.Recycle(obj);
            VFXRemove?.Invoke(obj);
        }

        protected override void onTick(float dt)
        {
            foreach (var obj in _vfxes)
            {
                if (obj.Value.IsExpired)
                {
                    _removeList.Add(obj.Value);
                }

                if (obj.Value.Setting.VFXBindType == EVFXType.FollowActor)
                {
                    obj.Value.Pos = Unit.UnitTransform.Pos + (Vector3)obj.Value.Setting.Offset;
                }

                obj.Value.OnTick(dt);
            }

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