using System.Collections.Generic;
using Unity.Collections;

namespace Hono.Scripts.Battle.Core {

    /// <summary>
    /// 任务重要程度类型
    /// </summary>
    public enum EQuestType
    {
        High,
        Middle,
        Lower,
    }
    
    /// <summary>
	/// 任务信息，纯静态数据
	/// </summary>
	public class QuestInfo
    {
        /// <summary>
        /// 任务Id
        /// </summary>
        public int QuestId;
        /// <summary>
        /// 主任务id
        /// </summary>
        public int MainQuestId;
        /// <summary>
        /// 任务重要程度
        /// </summary>
        public EQuestType QuestType;
        /// <summary>
        /// 显示排序
        /// </summary>
        public int ShowOrder;
        /// <summary>
        /// 任务标题
        /// </summary>
        public string Title;
        /// <summary>
        /// 任务描述
        /// </summary>
        public string Desc;
        /// <summary>
        /// 是否隐藏任务
        /// </summary>
        public bool IsHidden;
        /// <summary>
        /// 完成时完成主任务
        /// </summary>
        public bool FinishWithMain;
        /// <summary>
        /// 失败时主任务也失败
        /// </summary>
        public bool FailWithMain;
        /// <summary>
        /// 成功条件组合方式
        /// </summary>
        public bool FinishAllCond;
        /// <summary>
        /// 任务成功监听器
        /// </summary>
        public List<QuestCondMonitor> FinishMonitors = new(5);
        /// <summary>
        /// 失败条件组合方式
        /// </summary>
        public bool FailedAllCond;
        /// <summary>
        /// 任务失败监听器
        /// </summary>
        public List<QuestCondMonitor> FailMonitors = new(5);
        /// <summary>
        /// 任务开始时执行的动作
        /// </summary>
        public List<LevelAction> BeginActions = new(5);
        /// <summary>
        /// 任务完成时执行的动作
        /// </summary>
        public List<LevelAction> FinishActions = new(5);
        /// <summary>
        /// 任务失败时执行的动作
        /// </summary>
        public List<LevelAction> FailActions = new(5);
    }
}