using System;
using System.Collections.Generic;

namespace Hono.Scripts.Battle {
	public partial class ActorLogic {
		public class AttrCastLv1 : ALogicComponent {
			private List<int> _attrHeadList = new List<int>();

			public AttrCastLv1(ActorLogic logic) : base(logic) { }

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
							_attrHeadList.Add(attrId);
						}
					}
				}
			}

			protected override void onTick(float dt) {
				foreach (var headId in _attrHeadList) {
					calculateAttr(headId);
				}
			}

			public void calculateAttr(int headId) {
				var add = Actor.GetAttr<int>(headId + 2);
				var exAdd = Actor.GetAttr<int>(headId + 3);
				var per = Actor.GetAttr<int>(headId + 4);

				int final = (int)(add * (10000 + per)/10000f + exAdd);
				Actor.SetAttr<int>(headId+1, final, false);
				var total = Actor.GetAttr<int>(headId + 1);
				Actor.SetAttr<int>(headId, total, false);
			}
		}
	}
}