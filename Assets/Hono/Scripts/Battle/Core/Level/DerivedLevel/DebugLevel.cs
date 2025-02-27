using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Hono.Scripts.Battle.Core
{
    //要塞据点  参数：初始状态：未被占领，被某阵营占领 ；
    //             自动流转：兵长死亡时被占领，兵长死亡时被消灭）
    //        功能（可配置）:（产生特殊兵种，产生场地效果，产生世界效果），
    //        固定行为（场景对象死亡时固定发事件，据点被消灭事件，据点被占领事件）
    //交互物 状态：可见可交互，可见不可交互，不可见，）,
    //      行为：执行交互函数(获得物品，获得buff，获得技能，修改地图地形，发送自定义事件)
    //
    //触发器 状态：激活，非激活
    //      功能: 设置检测条件（tags，LevelObjectId）,设置触发时机（进入，离开）
    //      行为：满足条件后发送自定义事件
    //刷怪器 状态：活跃，静默
    //      功能：指定刷表中配置的怪物组，可配置初始id
    //      行为：刷怪
    //LevelActor 状态：已创建，未创建
    //           功能：创建一个指定的Actor
    //           行为：对象死亡时发送事件
    //LevelActorGroup 状态：已创建，未创建
    //              功能：创建指定的一群Actor,可设置队长，队长死亡时是否全部消灭
    //              行为：如果有队长则队长死亡时会触发事件，全成员死亡时会触发事件
    //监听某项数据是否发生，任务有Action(打开大门1)，有Listener(据点1，2，3被占领)
    
    //任务，监听固定事件，发送事件，设置任务状态，更改任务系统当前活跃的任务链条
    
    public class DebugLevel : Level
    {
        public override UniTask Load()
        {
            //加载关卡数据
            //Addressables.LoadAssetAsync<TextAsset>("")
            
            //场景对象初始化
            
            
            //任务系统初始化
            QuestSystem.Init(null);
            
            return UniTask.DelayFrame(1);
        }
    }
}