using System;
using UnityEngine;

namespace Battle.Tools.DebugTools
{
#if UNITY_EDITOR
    /// <summary>
    /// 战斗编辑器Debug组件
    /// </summary>
    public class AbilityDebugInspector : MonoBehaviour
    {
        public Actor DebugObj;

        private Actor.ActorDebugHandle _handle;

        public void OnEnable()
        {
            if (DebugObj != null)
            {
                _handle = DebugObj.DebugHandle;
            }
            //_handle.ActorHandle.AwardAbility();
        }

        private void Update()
        {
            if (_handle == null) return;
            
        }
    }
#endif
}