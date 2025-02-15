#region

using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Core.Base;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

#endregion

namespace Hono.Scripts.Battle
{
    [Serializable]
    public class VFXSetting
    {
        [LabelText("偏移量")]
        public SerializableVec3 offset;
        [LabelText("旋转值")]
        public SerializableVec3 rotOffset;
        [LabelText("缩放系数")]
        public float scale = 1;
        [LabelText("是否为世界特效")]
        public bool isWorldVFX;
        [LabelText("是否以世界原点为初始坐标(默认为玩家当前坐标)")]
        [ShowIf("isWorldVFX")]
        public bool isWorldPoint;
        [LabelText("绑定骨骼")]
        [HideIf("isWorldVFX")]
        public string boneName = "";
        [LabelText("不跟随父节点旋转")]
        [HideIf("isWorldVFX")]
        public bool notFollowParentRot;
        [LabelText("特效持续时长 -1为永久")] 
        public float duration;
    }
}