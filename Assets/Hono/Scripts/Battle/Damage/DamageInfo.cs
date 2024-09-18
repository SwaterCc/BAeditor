﻿using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public class DamageInfo
    {
        public int SourceActorId;
        public EAbilityType SourceAbilityType;
        public int SourceAbilityConfigId;

        /// <summary>
        /// 这个值如果是技能，就是技能基础倍率，如果是buff那就是子弹倍率
        /// </summary>
        public int BaseDamagePer;
        
        public List<int> Tags = new List<int>();

        /// <summary>
        /// 一共命中了几个目标
        /// </summary>
        public int HitCount;

        /// <summary>
        /// 这是第几次命中
        /// </summary>
        public int HitNumberCount;
		
        /// <summary>
        /// 是否必定暴击
        /// </summary>
        public bool IsCriticalOnce;
    }
}