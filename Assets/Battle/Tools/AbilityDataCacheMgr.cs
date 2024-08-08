using Battle.Def;

namespace Battle.Tools
{
    public class AbilityDataCacheMgr
    {
        private static AbilityDataCacheMgr _instance;

        public static AbilityDataCacheMgr Instance
        {
            get { return _instance ??= new AbilityDataCacheMgr(); }
        }

        /// <summary>
        /// 获取指定Id的data
        /// </summary>
        /// <param name="dataId"></param>
        /// <returns></returns>
        public AbilityData GetAbilityData(int dataId)
        {
            return new AbilityData();
        }
    }
}