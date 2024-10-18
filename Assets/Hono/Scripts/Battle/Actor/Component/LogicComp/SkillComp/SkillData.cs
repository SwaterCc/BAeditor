using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public class SkillData : ScriptableObject, IAllowedIndexing
    {
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
        public int MaxTargetCount = 1;//目标选择最大数量
        public bool SelectSelf = false;
		public string SkillName;
		public string SkillDesc;
		public string SkillIconPath;
	}
}