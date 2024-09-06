using UnityEngine;

namespace Hono.Scripts.Battle
{
    [CreateAssetMenu(menuName = "战斗编辑器/Actor/ActorShowData")] 
    public class ActorShowData : ScriptableObject,IAllowedIndexing
    {
        public int ID => id;
        public int id;
        public EActorShowType ShowType;
        public string ModelPath;
        public int InitAttrId;
    }
}