#region

using UnityEngine;
using UnityEngine.SceneManagement;

#endregion

namespace Hono.Scripts.Battle.Tools.DebugTools
{
    public class SceneSwitchButton : MonoBehaviour
    {
        public int SceneConfigId;

        public void OnButtonClick()
        {
            var scene = SceneManager.GetActiveScene();
            BattleManager.Instance.EnterWorld(scene.path, SceneConfigId);
        }
    }
}