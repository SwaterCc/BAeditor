using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Hono.Scripts.Battle.Tools;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Hono.Scripts.Battle
{
    public class GameObjectPreLoadMgr : Singleton<GameObjectPreLoadMgr> , IBattleFrameworkAsyncInit
    {
        private GameObject _bulletModel;
        public GameObject BulletModel => _bulletModel;
        
        private GameObject _hitBoxModel;
        
        public async UniTask AsyncInit()
        {
            List<UniTask> loadTasks = new List<UniTask>();
            
            loadTasks.Add(loadBulletModel());

            await UniTask.WhenAll(loadTasks);
        }

        private async UniTask loadBulletModel()
        {
            _bulletModel = await Addressables.LoadAssetAsync<GameObject>("Assets/BattleData/Tools/BulletModel.prefab").ToUniTask();
        }

    }
}