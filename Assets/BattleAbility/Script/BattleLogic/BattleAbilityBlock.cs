using System.Collections.Generic;

namespace BattleAbility
{
    /// <summary>
    /// 战斗能力类
    /// 通用的战斗逻辑执行模块管理战斗逻辑技能，buff，子弹的流程，基础数据来自配置，额外存有一个运行时数据类，需要池化
    /// </summary>
    public partial class BattleAbilityBlock
    {
        private readonly Dictionary<int, AbilityStage> _stages = new(7);
        private readonly BattleAbilityState _state;
        private readonly BattleAbilityData _data;

        public BattleAbilityBlock(BattleAbilityData data)
        {
            _data = data;
            foreach (var stageData in _data.stageDatas)
            {
                var newStage = new AbilityStage(this, stageData);
                _stages.Add(stageData.stageId, newStage);
            }
        }

        /// <summary>
        /// 从指定阶段开始运行
        /// </summary>
        public void Run(BattleAbilityState state)
        {
            _state.CurStageId = state.BeginStageId;
            _state.CurStage = _stages[state.BeginStageId];
            OnStart();
        }

        /// <summary>
        /// 重置数据
        /// </summary>
        public void Reset()
        {
        }

        //生命周期

        /// <summary>
        /// 能力启动，在run逻辑执行后同帧调用
        /// </summary>
        public void OnStart()
        {
            _state.CurStage.OnStart();
        }

        /// <summary>
        /// 块逻辑更新每次tick调用
        /// </summary>
        public void OnTick()
        {
            if (_state.State == EBattleAbilityState.UnInit || _state.State == EBattleAbilityState.Ready ||
                _state.State == EBattleAbilityState.LogicEnd)
            {
                return;
            }


            if (_state.State == EBattleAbilityState.StageEnd)
            {
                _state.CurStage.OnEnd();
                if (_state.NextStageId != -1)
                {
                    _state.CurStageId = _state.NextStageId;
                    _state.NextStageId = -1;
                    _state.CurStage = _stages[_state.CurStageId];
                    _state.CurStage.OnStart();
                }
                else
                {
                    _state.State = EBattleAbilityState.LogicEnd;
                }
            }

            if (_state.State == EBattleAbilityState.StageRunning)
            {
                _state.CurStage.OnTick();
            }
        }

        /// <summary>
        /// 能力阶段结束，删除之前调用
        /// </summary>
        public void OnEnd()
        {
        }
    }
}