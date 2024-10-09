using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hono.Scripts.Battle
{
    [Serializable]
    public class ActorGroup
    {
        [ReadOnly]
        public int GroupId;
        public int Place0;
        public int Place1;
        public int Place2;
        public int Place3;

        public int this[int idx]
        {
            get { 
                idx = Mathf.Clamp(idx, 0, BattleConstValue.PawnGroupMemberMaxCount);
                switch (idx)
                {
                    case 0:
                        return Place0;
                    case 1:
                        return Place1;
                    case 2:
                        return Place2;
                    case 3:
                        return Place3;
                }

                return -1;
            }
        }
        
        public bool IsEmpty()
        {
            return Place0 + Place1 + Place2 + Place3 > 0;
        }
    }

    [Serializable]
    public class ActorGroupInfos
    {
        public ActorGroup Group0 = new ActorGroup() { GroupId = 0 };
        public ActorGroup Group1 = new ActorGroup() { GroupId = 1 };
        public ActorGroup Group2 = new ActorGroup() { GroupId = 2 };
        public ActorGroup Group3 = new ActorGroup() { GroupId = 3 };


        public ActorGroup this[int idx]
        {
            get { 
                idx = Mathf.Clamp(idx, 0, BattleConstValue.PawnGroupMaxCount);
                switch (idx)
                {
                    case 0:
                        return Group0;
                    case 1:
                        return Group1;
                    case 2:
                        return Group2;
                    case 3:
                        return Group3;
                }

                return null;
            }
        }
    }
}