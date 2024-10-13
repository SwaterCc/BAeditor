using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle {
	public partial class ActorLogic {
		/// <summary>
		/// 视觉特效
		/// </summary>
		public class VFXComp : ALogicComponent {
			protected readonly CommonUtility.IdGenerator _idGenerator = CommonUtility.GetIdGenerator();
			private Dictionary<int,VFXObject> _vfxs = new();
			public Dictionary<int,VFXObject> VFXObjects => _vfxs;

			private List<VFXObject> _removeList = new();
			
			public Action<VFXObject> VFXAdd;
			public Action<VFXObject> VFXRemove;

			public VFXComp(ActorLogic logic) : base(logic) { }
			public override void Init() { }

			public int AddVFXObject(VFXSetting setting) {
				var vfxObj = new VFXObject(_idGenerator.GenerateId(), setting);

				switch (setting.VFXBindType) {
					case EVFXType.InWorld:
						vfxObj.Pos = Actor.Pos + Actor.Rot * setting.Offset;
						vfxObj.Rot = Actor.Rot * Quaternion.Euler(setting.Rot);
						break;
					case EVFXType.FollowActor:
						vfxObj.Pos = Actor.Pos + (Vector3)setting.Offset;
						vfxObj.Rot = Quaternion.Euler(setting.Rot);
						break;
					case EVFXType.BindActorBone:
						vfxObj.Pos = setting.Offset;
						vfxObj.Rot = Quaternion.Euler(setting.Rot);
						break;
				}
				
				
				if (setting.VFXBindType != EVFXType.InWorld) {
					AddVFXToList(vfxObj);
				}
				else {
					BattleManager.battleController.AddWorldVFX(vfxObj);
				}

				return vfxObj.Uid;
			}

			protected void AddVFXToList(VFXObject vfxObj) {
				_vfxs.Add(vfxObj.Uid,vfxObj);
				VFXAdd?.Invoke(vfxObj);
			}

			public void RemoveVFX(int key) {
				if (_vfxs.TryGetValue(key,out var obj)) {
					onRemove(obj);
				}
			}

			protected void onRemove(VFXObject obj) {
				_vfxs.Remove(obj.Uid);
				VFXRemove?.Invoke(obj);
			}
			
			protected override void onTick(float dt) {
				foreach (var obj in _vfxs) {
					if (obj.Value.IsExpired) {
						_removeList.Add(obj.Value);
					}

					if (obj.Value.Setting.VFXBindType == EVFXType.FollowActor) {
						obj.Value.Pos = Actor.Pos + (Vector3)obj.Value.Setting.Offset;
					}
					obj.Value.OnTick(dt);
				}
				
				foreach (var obj in _removeList) {
					onRemove(obj);
				}
			}
		}
	}
}