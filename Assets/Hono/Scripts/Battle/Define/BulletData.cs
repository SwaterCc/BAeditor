using System;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public class BulletData : ScriptableObject, IAllowedIndexing
    {
        public int ID => Id;
        public int Id;
        
        public bool CustomMotion;
        public EMotionType MotionType = EMotionType.Liner;

        public Vector3 Offset;
        
        public bool CloseFollowTarget;
        
        public float BulletSpeed;
        public bool IsHitPathActor;
        public int DamageConfigId;
        public float BulletLifeTime;
        public int MaxHitCount;
        
        public FilterSetting FilterSetting;
    }
}