using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Hono.Scripts.Battle.Tools.DebugTools
{
    public class SceneButton : MonoBehaviour
    {
        public int SceneConfigId;
        public void OnButtonClick() {
	        var scene = SceneManager.GetActiveScene();
	        
            BattleManager.Instance.EnterBattle(scene.path, SceneConfigId);
        }
    }
}