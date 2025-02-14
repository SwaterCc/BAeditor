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
    public enum EVFXType
    {
        InWorld,
        FollowActor,
        BindActorBone,
    }

    [Serializable]
    public class VFXSetting
    {
        public bool isUsePE;
        public SerializableVec3 offset;
        public SerializableVec3 rot;
        public float scale = 1;
        public EVFXType vfxBindType;
        [ShowIf("VFXBindType", EVFXType.BindActorBone)]
        public string boneName = "";
        [LabelText("特效持续时长 -1为永久")] 
        public float duration;
    }
}