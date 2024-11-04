#region

using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    public class MonsterGeneratorModel : SceneActorModel
    {
        public List<Transform> WayPoint = new();

        protected override void onInit()
        {
            ActorType = EActorType.MonsterGenerator;
            name = $"MonsterGenerator:{ActorUid}";
        }

        public Rect GetRect()
        {
            return new Rect(transform.position.x, transform.position.z, transform.localScale.x / 2f,
                transform.localScale.z / 2f);
        }

        public void GetWayPoint(ref List<Vector3> wayPoints)
        {
            foreach (var trans in WayPoint)
            {
                wayPoints.Add(trans.position);
            }
        }
    }
}