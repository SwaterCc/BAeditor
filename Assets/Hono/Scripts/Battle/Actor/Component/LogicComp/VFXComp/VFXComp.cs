#region

using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Tools;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    public partial class ActorLogic
    {
        /// <summary>
        ///     视觉特效
        /// </summary>
        public class VFXComp : AComponent
        {
            //特效组件的唯一Id生成器
            private static readonly CommonUtility.IdGenerator IDGenerator = CommonUtility.GetIdGenerator();

            private readonly Dictionary<int, VFXObject> _vfxes = new(32);
            public Dictionary<int, VFXObject> VFXDict => _vfxes;

            private readonly List<VFXObject> _removeList = new(32);

            public Action<VFXObject> VFXAdd;
            public Action<VFXObject> VFXRemove;

            public VFXComp(ActorLogic logic) : base(logic) { }
            public override void Init() { }

            public int AddVFXObject(VFXSetting setting)
            {
                var vfxObj = APool<VFXObject>.Pool.Rent();
                vfxObj.OnRent(IDGenerator.GenerateId(), setting);

                switch (setting.VFXBindType)
                {
                    case EVFXType.InWorld:
                        vfxObj.Pos = Self.Pos + Self.Rot * setting.Offset;
                        vfxObj.Rot = Self.Rot * Quaternion.Euler(setting.Rot);
                        break;
                    case EVFXType.FollowActor:
                        vfxObj.Pos = Self.Pos + (Vector3)setting.Offset;
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
                    BattleManager.BattleController.VFXComp.AddVFXToList(vfxObj);
                }

                return vfxObj.Uid;
            }

            protected void AddVFXToList(VFXObject vfxObj)
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

            protected void onRemove(VFXObject obj)
            {
                _vfxes.Remove(obj.Uid);
                APool<VFXObject>.Pool.Recycle(obj);
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
                        obj.Value.Pos = Self.Pos + (Vector3)obj.Value.Setting.Offset;
                    }

                    obj.Value.OnTick(dt);
                }

                foreach (var obj in _removeList)
                {
                    onRemove(obj);
                }

                _removeList.Clear();
            }

            public override void Clear()
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
}