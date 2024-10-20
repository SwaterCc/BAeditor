using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class ActorModelController
    {
        public class CustomModelSetup : ModelSetup
        {
            public void SetPath(string path)
            {
                _path = path;
            }

            public void SetModel(GameObject obj)
            {
                _gameObject = obj;
            }

            protected override void OnLoadModel()
            {
                _loadComplete.Invoke(_gameObject);
            }

            protected override void OnUnInit() { }
        }
    }
}