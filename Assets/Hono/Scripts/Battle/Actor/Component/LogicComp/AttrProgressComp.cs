using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle {
	public partial class ActorLogic {
		public class AttrSimpleProgress : ALogicComponent {
			private List<int> _attrHeadList = new List<int>();
			private float _dt;
			public AttrSimpleProgress(ActorLogic logic) : base(logic) { }

			public override void Init() {
				var values = Enum.GetValues(typeof(ELogicAttr));


				foreach (var value in values) {
					int attrId = (int)value;

					if (attrId > 10000 && attrId % 10 == 0) {
						var isDefined = Enum.IsDefined(typeof(ELogicAttr), attrId + 1);
						isDefined = isDefined && Enum.IsDefined(typeof(ELogicAttr), attrId + 2);
						isDefined = isDefined && Enum.IsDefined(typeof(ELogicAttr), attrId + 3);
						isDefined = isDefined && Enum.IsDefined(typeof(ELogicAttr), attrId + 4);
						isDefined = isDefined && Enum.IsDefined(typeof(ELogicAttr), attrId + 5);
						if (isDefined) {
							if (attrId == 10120 || attrId == 10130 ||attrId == 10140) {
								continue;
							}
							_attrHeadList.Add(attrId);
						}
					}
				}

				_dt = 2;
			}

			protected override void onTick(float dt) {
				if (_dt > 1f) {
					foreach (var headId in _attrHeadList) {
						calculateAttr(headId);
					}

					_dt = 0;
				}

				_dt += dt;
			}

			private void calculateAttr(int headId) {
				var add = Actor.GetAttr<int>(headId + 2);
				var exAdd = Actor.GetAttr<int>(headId + 3);
				var per = Actor.GetAttr<int>(headId + 4);

				int final = (int)(add * ((10000 + per) / 10000f) + exAdd);
				Actor.SetAttr<int>(headId+1, final, false);
				var total = Actor.GetAttr<int>(headId + 1);
				Actor.SetAttr<int>(headId, total, false);
			}
		}
	}
}