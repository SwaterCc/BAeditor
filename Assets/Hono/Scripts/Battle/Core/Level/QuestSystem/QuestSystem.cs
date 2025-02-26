using System.Collections.Generic;
using System.Linq;
using Hono.Scripts.Battle.Core.Base;
using Newtonsoft.Json.Linq;
using Unity.Collections;
using UnityEngine;

namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// 任务控制器
    /// </summary>
    public class QuestSystem
    {
        /// <summary>
        /// 当前关卡
        /// </summary>
        private Level _level;

        /// <summary>
        /// 待接取的任务
        /// </summary>
        private HashSet<int> _awaitingAcceptQuests;

        /// <summary>
        /// 在执行的任务
        /// </summary>
        private HashSet<int> _executeQuests;

        /// <summary>
        /// 已完成的任务
        /// </summary>
        private HashSet<int> _finishQuests;

        /// <summary>
        /// 所有任务数据
        /// </summary>
        private readonly Dictionary<int, QuestInfo> _questInfos;

        /// <summary>
        /// 临时存储用的列表
        /// </summary>
        private List<QuestCondMonitor> _tempAcceptCondMonitorList;

        public QuestSystem(Level level)
        {
            _level = level;
            _questInfos = new Dictionary<int, QuestInfo>();
            _tempAcceptCondMonitorList = new List<QuestCondMonitor>(10);
        }

        public void Init(JObject mainQuest)
        {
            //解析任务，将无接取条件的任务自动接取，创建所有监听器
            if (mainQuest["subQuests"] is not JArray subQuests)
            {
                Debug.Log("子任务列表为空！");
                return;
            }

            foreach (var quest in subQuests)
            {
                parseQuestInfo(quest as JObject);
            }
        }

        private void parseQuestInfo(JObject quest)
        {
            var info = new QuestInfo
            {
                QuestId = (int)(quest["subId"] ?? 0),
                MainQuestId = (int)(quest["mainId"] ?? 0),
                QuestType = quest["type"]!.ToObject<EQuestType>(),
                ShowOrder = (int)(quest["order"] ?? 0),
                Title = (string)(quest["titleText"] ?? ""),
                Desc = (string)(quest["descText"] ?? ""),
                IsHidden = (bool)(quest["isHidden"] ?? false),
                FinishWithMain = (bool)(quest["finishParent"] ?? false),
                FailWithMain = (bool)(quest["failParent"] ?? false),
                FinishAllCond = (bool)(quest["finishAllCond"] ?? false),
                FailedAllCond = (bool)(quest["failAllCond"] ?? false),
            };

            //解析接受条件
            parseCondMonitor(quest["acceptCond"] as JArray, ref _tempAcceptCondMonitorList);
            parseCondMonitor(quest["finishCond"] as JArray, ref info.FinishMonitors);
            parseCondMonitor(quest["failCond"] as JArray,   ref info.FailMonitors);
            //解析行为
            parseLevelAction(quest["beginExec"] as JArray,  ref info.BeginActions);
            parseLevelAction(quest["finishExec"] as JArray, ref info.FinishActions);
            parseLevelAction(quest["failExec"] as JArray,   ref info.FailActions);
            _questInfos.Add(info.QuestId, info);
            if (_tempAcceptCondMonitorList.Count == 0)
            {
                QuestAccept(info.QuestId);
            }
            else
            {
                foreach (var condMonitor in _tempAcceptCondMonitorList)
                {
                    _level.AddMonitor(condMonitor);
                }

                _tempAcceptCondMonitorList.Clear();
            }
        }

        private void parseCondMonitor(JArray array, ref List<QuestCondMonitor> monitors) { }

        private void parseLevelAction(JArray array, ref List<LevelAction> actions) { }

        /// <summary>
        /// 任务条件检测通过
        /// </summary>
        /// <param name="questId"></param>
        /// <param name="questMonitorType"></param>
        public void OnQuestCondMonitorPass(int questId, EQuestStateType questMonitorType)
        {
            var info = _questInfos[questId];
            switch (questMonitorType)
            {
                case EQuestStateType.Accept:
                    QuestAccept(questId);
                    break;
                case EQuestStateType.Finish:
                    if (info.FinishAllCond)
                    {
                        if (info.FinishMonitors.All(monitor => monitor.IsPass))
                        {
                            QuestFinish(questId);
                        }
                    }
                    else
                    {
                        QuestFinish(questId);
                    }

                    break;
                case EQuestStateType.Fail:
                    if (info.FailedAllCond)
                    {
                        if (info.FinishMonitors.All(monitor => monitor.IsPass))
                        {
                            QuestFailed(questId);
                        }
                    }
                    else
                    {
                        QuestFailed(questId);
                    }

                    break;
            }
        }

        /// <summary>
        /// 接取任务
        /// </summary>
        /// <param name="questId"></param>
        private void QuestAccept(int questId)
        {
            //注册完成条件监听
            var info = _questInfos[questId];
            foreach (var monitor in info.FinishMonitors)
            {
                _level.AddMonitor(monitor);
            }

            //失败条件监听
            foreach (var monitor in info.FailMonitors)
            {
                _level.AddMonitor(monitor);
            }

            _awaitingAcceptQuests.Remove(questId);
            _executeQuests.Add(questId);
        }

        /// <summary>
        /// 完成任务
        /// </summary>
        /// <param name="questId"></param>
        private void QuestFinish(int questId)
        {
            //注册完成条件监听
            var info = _questInfos[questId];
            foreach (var action in info.FinishActions)
            {
                action.DoAction();
            }

            _executeQuests.Remove(questId);
            _finishQuests.Add(questId);

            if (info.FinishWithMain)
            {
                Debug.Log($"主任务{info.MainQuestId} 完成");
            }
        }

        private void QuestFailed(int questId)
        {
            //注册完成条件监听
            var info = _questInfos[questId];
            foreach (var action in info.FailActions)
            {
                action.DoAction();
            }

            _executeQuests.Remove(questId);
            if (info.FailWithMain)
            {
                Debug.Log($"主任务{info.MainQuestId} 失败");
            }
        }
    }
}