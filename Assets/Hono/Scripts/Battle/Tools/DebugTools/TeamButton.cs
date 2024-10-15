using System;
using UnityEngine;

namespace Hono.Scripts.Battle.Tools.DebugTools
{
    public class TeamButton : MonoBehaviour
    {
        public PawnTeamAllData TeamAllData;

        private Action<IUIPassData> _onUICloseCallBack;

        public BuildTeamsUIRoot Root;
        
        public void OnClick()
        {
            Root.AllData = TeamAllData;
            Root.gameObject.SetActive(false);
        }
    }
}