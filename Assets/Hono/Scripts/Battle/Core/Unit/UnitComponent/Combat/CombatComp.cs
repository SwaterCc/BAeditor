using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// 战斗组件
    /// </summary>
    public partial class CombatComp : UnitComponent
    {
        /// <summary>
        /// 技能控制器
        /// </summary>
        private readonly SkillDriver _skillDriver;
        /// <summary>
        /// buff控制器
        /// </summary>
        private readonly BuffController _buffController;
        /// <summary>
        /// 战斗资源管理
        /// </summary>
        private readonly CombatResourceCtrl _resourceCtrl;
        
        public CombatComp()
        {
            _skillDriver = new SkillDriver(this);
            _buffController = new BuffController(this);
        }

        public override void Init()
        {
            //添加buff
            //添加技能
            //运行被动技能
        }

        protected override void onTick(float dt)
        {
            _skillDriver.Tick(dt);
            _buffController.Tick(dt);
            _resourceCtrl.Tick(dt);
        }

        protected override void onClear()
        {
            _skillDriver.Clear();
            _buffController.Clear();
            _resourceCtrl.Clear();
        }
        //技能不做目标选择，技能仅接受目标，目标的选择来自上层传入，选择的方式由技能配置（方向（子弹用），坐标（AOE），目标(单体技能或AOE)，无（逻辑里自选））
        //提供接口获取目标返回坐标，获取目标返回UID，获取目标返回方向向量
        //

        #region Skill

        /// <summary>
        /// 使用技能
        /// </summary>
        public void UseSkill(int skillId)
        {
            _skillDriver.TryUseSkill(skillId);
        }

        /*public Skill GetSkill()
        {
            
        }*/

        #endregion
        


        #region Buff

        public void AddBuff(int buffId,int sourceUnitUid,  int buffLayer = 1)
        {
            if (!AssetManager.Instance.TryGetData<BuffData>(buffId, out var buffData))
            {
                Debug.LogError($"找不到指定Buff:{buffId}数据");
                return;
            }

            _buffController.AddBuff(sourceUnitUid, buffId, buffLayer, buffData);
        }
        
        public int GetBuffLayer(int buffId, int sourceUnitUid = -1)
        {
            return _buffController.GetBuffLayer(sourceUnitUid, buffId);
        }
        
        public void RemoveBuff(int buffId, int sourceUnitUid)
        {
            _buffController.RemoveBuff(sourceUnitUid, buffId);
        }

        /// <summary>
        /// 执行buff ability
        /// </summary>
        private void onBuffAdd(ref Buff buff) { }

        private void onBuffOverride(ref Buff buff) { }

        private void onBuffLayering(ref Buff buff) { }

        private void onBuffRemove(ref Buff buff) { }

        #endregion
    }
}