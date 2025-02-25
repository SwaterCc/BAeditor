using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Hono.Scripts.Battle.Core
{
    public class DebugLevel : Level
    {
        public override UniTask Load()
        {
            //加载关卡数据
            //Addressables.LoadAssetAsync<TextAsset>("")
            //场景对象列表
            
            
            
            //任务列表
            
            
            return UniTask.DelayFrame(1);
        }
    }
}