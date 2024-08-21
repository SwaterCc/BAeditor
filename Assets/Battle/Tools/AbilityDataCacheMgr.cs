using System.Collections.Generic;
using AbilityRes;

namespace Battle.Tools
{
    public class AbilityDataCacheMgr
    {
        private static AbilityDataCacheMgr _instance;

        public static AbilityDataCacheMgr Instance
        {
            get { return _instance ??= new AbilityDataCacheMgr(); }
        }

        private Dictionary<int, AbilityData> _abilityDatas = new Dictionary<int, AbilityData>(64);

        private AbilityDataList _skill;
        private AbilityDataList _buff;
        private AbilityDataList _bullet;
        
        public void Init()
        {
          
        }
        
        /// <summary>
        /// 获取指定Id的data
        /// </summary>
        /// <param name="dataId"></param>
        /// <returns></returns>
        public AbilityData GetAbilityData(int dataId)
        {
            //临时用直接加载
            return new AbilityData();
        }
    }
}