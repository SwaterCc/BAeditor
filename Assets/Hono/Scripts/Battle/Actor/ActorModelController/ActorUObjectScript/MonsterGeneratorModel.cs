using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public class MonsterGeneratorModel : SceneActorModel
    {
        public List<Transform> WayPoint = new ();
        
        protected override void onInit()
        {
            ActorType = EActorType.MonsterGenerator;
        }

        public List<Vector3> GetWayPoint()
        {
            var result = new List<Vector3>();

            foreach (var trans in WayPoint)
            {
                result.Add(trans.position);
            }
            
            return result;
        }
    }
}