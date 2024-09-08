using Sirenix.OdinInspector;
using UnityEngine;

namespace Hono.Scripts.Battle.Tools.DebugTools
{
    public class ConfigTest : MonoBehaviour
    {
        [Button]
        public void Init()
        {
            ConfigManager.Instance.Init();
        }
    }
}