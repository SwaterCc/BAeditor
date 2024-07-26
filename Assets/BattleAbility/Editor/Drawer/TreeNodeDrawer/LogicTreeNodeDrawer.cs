using System.Collections.Generic;

namespace BattleAbility.Editor
{
    /// <summary>
    /// 绘制树节点的抽象类
    /// </summary>
    public abstract class LogicTreeNodeDrawer
    {
        //private 
        private LogicTreeNodeDrawer _parent = null;
        private List<LogicTreeNodeDrawer> _children = new();
    }
}