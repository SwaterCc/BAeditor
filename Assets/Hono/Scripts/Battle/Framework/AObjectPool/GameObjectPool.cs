using System.Collections.Generic;
using Hono.Scripts.Battle.Tools;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public class GameObjectPool : MonoSingleton<GameObjectPool>
    {
        private readonly Dictionary<string, Queue<GameObject>> _gameObjectCache = new();
        
        public bool TryGet(string path, out GameObject obj)
        {
            obj = null;

            if (_gameObjectCache.TryGetValue(path, out var objects))
            {
                if (objects.Count == 0)
                {
                    return false;
                }

                obj = objects.Dequeue();
                obj.SetActive(true);
                return true;
            }

            return false;
        }

        public void Recycle(string path, in GameObject obj)
        {
            obj.transform.parent = transform;
            obj.SetActive(false);

            if (!_gameObjectCache.TryGetValue(path, out var objects))
            {
                objects = new Queue<GameObject>(16);
                _gameObjectCache.Add(path, objects);
            }

            objects.Enqueue(obj);
        }
        
    }
}