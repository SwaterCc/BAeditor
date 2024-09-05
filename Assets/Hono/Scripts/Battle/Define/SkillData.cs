using System.Collections.Generic;
using Hono.Scripts.Battle.Tools;
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
        public List<ResItems> SkillResCheck;
        public EResCostType CostType;
        public List<ResItems> SkillResCost;
        public List<int> Tags = new List<int>();
        public int PriorityATK;
        public int PriorityDEF;
        public bool ForceFaceTarget;
        public int SkillDamageBasePer; //基础倍率，万分比
        public bool UseCustomFilter;
        public FilterSetting CustomFilter;
    }
}