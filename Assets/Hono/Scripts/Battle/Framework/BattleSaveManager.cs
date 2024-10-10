using System;
using Cysharp.Threading.Tasks;
using Hono.Scripts.Battle.Tools;
using Object = UnityEngine.Object;

namespace Hono.Scripts.Battle
{
    public class BattleSaveManager: Singleton<BattleSaveManager>,IBattleFrameworkAsyncLoad
    {
        private BattleGameSave _gameSave;
        public BattleGameSave GameSave => _gameSave;
        
        public UniTask AsyncLoad()
        {
            //加载所有存档
            return new UniTask();
        }
        
        /// <summary>
        /// 是否有存档
        /// </summary>
        /// <returns></returns>
        public bool HasSave(string saveName)
        {
            return false;
        }

        /// <summary>
        /// 尝试获取存档
        /// </summary>
        /// <param name="saveName"></param>
        /// <param name="gameSave"></param>
        /// <returns></returns>
        public bool TryGetSaveData(string saveName ,out BattleGameSave gameSave)
        {
            gameSave = null;
            return false;
        }

        /// <summary>
        /// 存储数据
        /// </summary>
        public void Save()
        {
            
        }
    }
}