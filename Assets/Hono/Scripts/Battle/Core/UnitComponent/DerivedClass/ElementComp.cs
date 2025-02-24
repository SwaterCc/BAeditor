using System.Collections.Generic;

namespace Hono.Scripts.Battle.Core
{
    public class ElementComp : UnitComponent,IGPoolObject
    {
        /// <summary>
        /// 元素异常关系链
        /// </summary>
        private readonly static List<ElementLink> ElementLinks = new()
        {
            new(EAttrType.AttrWaterElementStackAdd, EAttrType.AttrWaterElementHp, EAttrType.AttrWaterElementMaxHp,
                1),
            new(EAttrType.AttrFireElementStackAdd, EAttrType.AttrFireElementHp, EAttrType.AttrFireElementMaxHp, 203201),
            new(EAttrType.AttrWindElementStackAdd, EAttrType.AttrWindElementHp, EAttrType.AttrWindElementMaxHp, 1),
            new(EAttrType.AttrRockElementStackAdd, EAttrType.AttrRockElementHp, EAttrType.AttrRockElementMaxHp, 1),
            new(EAttrType.AttrLightElementStackAdd, EAttrType.AttrLightElementHp, EAttrType.AttrLightElementMaxHp,
                1),
            new(EAttrType.AttrDarkElementStackAdd, EAttrType.AttrDarkElementHp, EAttrType.AttrDarkElementMaxHp, 1),
        };

        
        public override void Init()
        {
          
        }

        public override void Recycle()
        {
           GPool<ElementComp>.Pool.Recycle(this);
        }

        public void OnRecycle()
        {
           
        }
        
        
        public void CumulativeElementValue(Unit attacker, DamageTableRow damageRow)
        {
            //无元素类型不处理
            if (damageRow.ElementsDamage.Count != 2)
            {
                return;
            }

            foreach (var link in ElementLinks)
            {
                link.Process(attacker, Unit, damageRow);
            }
        }

    }
}