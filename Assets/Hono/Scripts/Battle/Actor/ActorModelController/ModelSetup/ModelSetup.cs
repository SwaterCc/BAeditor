using System;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class ActorModelController
    {
        public abstract class ModelSetup
        {
            protected GameObject _gameObject;
            
            protected string _path;

            protected ActorModelController _modelController;

            protected Action<GameObject> _loadComplete;
            
            public void LoadModel(ActorModelController modelController, Action<GameObject> loadComplete)
            {
                _modelController = modelController;
                _loadComplete = loadComplete;
                OnLoadModel();
            }

            protected abstract void OnLoadModel();
            
            protected abstract void OnUnInit();
            
            public void UnInit()
            {
                OnUnInit();
                if (!string.IsNullOrEmpty(_path) && _gameObject != null)
                {
                    GameObjectPool.Instance.Recycle(_path, _gameObject);
                }

                _gameObject = null;
                _path = null;
            }
        }
    }
}