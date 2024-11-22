#region

using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle {
	public class SkillData : ScriptableObject, IAllowedIndexing {
		public int ID => SkillId;
		public int SkillId;
		public ESkillType SkillType;
		public ESkillTargetType SkillTargetType;
		public float SkillRange;
		public ECDMode EcdMode;
		public float SkillCD;
		public List<ResItems> SkillResCheck = new();
		public EResCostType CostType;
		public List<ResItems> SkillResCost = new();
		public int PriorityATK;
		public int PriorityDEF;
		public bool ForceFaceTarget;
		public int SkillDamageBasePer; //基础倍率，万分比
		public bool UseCustomFilter = true;
		public FilterSetting CustomFilter;
		public int MaxTargetCount = 1; //目标选择最大数量
		public bool SelectSelf = false;
		public string SkillName;
		public string SkillDesc;
		public string SkillIconPath;
		//技能的持续时长
		public float SkillDuration;
		//技能最短持续时长
		public float SkillMinInterval;
		//TODO:目前技能的实现还是太过于偏向ARPG了，对于RTS来讲技能的攻速其实更体现在攻击的间隔上，动作本身的速度并不会被影响
	}
}