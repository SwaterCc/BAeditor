using UnityEngine;

namespace Hono.Scripts.Battle
{
    /// <summary>
    /// 游戏存档
    /// </summary>
    public class BattleGameSave : ScriptableObject
    {
        /// <summary>
        /// 是否为有效数据
        /// </summary>
        private bool _hasEffectData;


        public void Save()
        {
            _hasEffectData = true;
        }
        
        public void ClearSave()
        {
            _hasEffectData = false;
        }
        
        public bool IsEmptySave()
        {
            return _hasEffectData;
        }
    }
}