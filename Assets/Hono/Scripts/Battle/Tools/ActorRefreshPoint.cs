using Sirenix.OdinInspector;
using UnityEngine;

namespace Hono.Scripts.Battle.Tools
{
    public enum EPointType
    {
        Player,
        Monster,
        HitBox,
    }
    
    public class ActorRefreshPoint : MonoBehaviour
    {
        public EPointType PointType;
        public int ActorId;
        
        [Button("创建Actor")]
        public void CreateActor()
        {
           var actor = ActorManager.Instance.CreateActor(ActorId);
           actor.Logic.SetAttr(ELogicAttr.AttrPosition, transform.position, false);
           actor.Logic.SetAttr(ELogicAttr.AttrRot, transform.rotation, false);
           ActorManager.Instance.AddActor(actor);
        }
    }
}