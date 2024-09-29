using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class EntityAttrBaseTable : ITableHelper
    {
        private readonly Dictionary<int, EntityAttrBaseRow> _tableData = new();

        public bool LoadCSV(string csvFile)
        {
            try
            {
                using (StringReader  reader = new StringReader (csvFile))
                {
                    int lineCount = 0;
                    while (reader.ReadLine() is { } line)
                    {
                        if (++lineCount <= 3)
                        {
                            continue;
                        }
                        var row = Activator.CreateInstance<EntityAttrBaseRow>();
                        row.Parser.Parse(line);
                        addRow(row.Id, row);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }

            return true;
        }

        public TableRow GetTableRow(int id)
        {
            if (TryGet(id, out var row))
            {
                return row;
            }

            return null;
        }

        private void addRow(int id, EntityAttrBaseRow row)
        {
            if (!_tableData.TryAdd(id, row))
            {
                Debug.LogError($"{typeof(EntityAttrBaseRow)} TryAdd {id} id重复");
            }
        }

        public EntityAttrBaseRow Get(int id)
        {
            return _tableData[id];
        }

        public bool TryGet(int id, out EntityAttrBaseRow data)
        {
            return _tableData.TryGetValue(id, out data);
        }

       
    }

    public partial class EntityAttrBaseTable
    {
        public class EntityAttrBaseRow : TableRow
        {
           
            /// <summary>
            /// 属性描述
            /// </summary>
            public string Desc { get; private set; }
            
            /// <summary>
            /// 默认初始等级，目标单位等级，为角色时该等级=主职业等级，为怪物时即正常等级。
            /// </summary>
            public int AttrEntityLevel { get; private set; }
            
            /// <summary>
            /// 最大HP
            /// </summary>
            public int AttrMaxHpAdd { get; private set; }
            
            /// <summary>
            /// 最大MP
            /// </summary>
            public int AttrMaxMpAdd { get; private set; }
            
            /// <summary>
            /// 攻击力
            /// </summary>
            public int AttrAttackAdd { get; private set; }
            
            /// <summary>
            /// 防御力
            /// </summary>
            public int AttrDefenseAdd { get; private set; }
            
            /// <summary>
            /// 防御穿透
            /// </summary>
            public int AttrIgnoreDefenseAdd { get; private set; }
            
            /// <summary>
            /// 防御穿透万分比
            /// </summary>
            public int AttrIgnoreDefensePCTAdd { get; private set; }
            
            /// <summary>
            /// 暴击率加值万分比
            /// </summary>
            public int AttrCritAdd { get; private set; }
            
            /// <summary>
            /// 暴击伤害加值万分比
            /// </summary>
            public int AttrCritDamageAdd { get; private set; }
            
            /// <summary>
            /// 治疗强度
            /// </summary>
            public int AttrHealIntensityAdd { get; private set; }
            
            /// <summary>
            /// 治疗效果
            /// </summary>
            public int AttrHealAdd { get; private set; }
            
            /// <summary>
            /// 被治疗效果
            /// </summary>
            public int AttrHealedAdd { get; private set; }
            
            /// <summary>
            /// 攻击速度
            /// </summary>
            public int AttrAttackSpeedPCTAdd { get; private set; }
            
            /// <summary>
            /// 全属性抗性穿透
            /// </summary>
            public int AttrElementPenPCTAdd { get; private set; }
            
            /// <summary>
            /// 全属性抗性
            /// </summary>
            public int AttrElementRedPCTAdd { get; private set; }
            
            /// <summary>
            /// 物理属性抗性穿透
            /// </summary>
            public int AttrElementPhysicalPenPCTAdd { get; private set; }
            
            /// <summary>
            /// 物理属性抗性
            /// </summary>
            public int AttrElementPhysicalRedPCTAdd { get; private set; }
            
            /// <summary>
            /// 元素属性抗性穿透
            /// </summary>
            public int AttrElementMagicPenPCTAdd { get; private set; }
            
            /// <summary>
            /// 元素属性抗性
            /// </summary>
            public int AttrElementMagicRedPCTAdd { get; private set; }
            
            /// <summary>
            /// 伤害增加万分比%[x]
            /// </summary>
            public int AttrDmgAAdd { get; private set; }
            
            /// <summary>
            /// 直接减伤%[x]
            /// </summary>
            public int AttrDmgRedAdd { get; private set; }
            
            /// <summary>
            /// CDR万分比
            /// </summary>
            public int AttrSkillCDPCTAdd { get; private set; }
            

            public EntityAttrBaseRow()
            {
                Parser = new EntityAttrBaseRowCSVParser(this);
            }

            private class EntityAttrBaseRowCSVParser : CSVParser
            {
                private EntityAttrBaseRow _row;

                public EntityAttrBaseRowCSVParser(EntityAttrBaseRow row) : base(row)
                {
                    _row = (EntityAttrBaseRow)base._row;
                }

                protected override void onParse(string[] line)
                {
                    
                    _row.Desc = parseString(line[1]);
            
                    _row.AttrEntityLevel = parseInt(line[2]);
            
                    _row.AttrMaxHpAdd = parseInt(line[3]);
            
                    _row.AttrMaxMpAdd = parseInt(line[4]);
            
                    _row.AttrAttackAdd = parseInt(line[5]);
            
                    _row.AttrDefenseAdd = parseInt(line[6]);
            
                    _row.AttrIgnoreDefenseAdd = parseInt(line[7]);
            
                    _row.AttrIgnoreDefensePCTAdd = parseInt(line[8]);
            
                    _row.AttrCritAdd = parseInt(line[9]);
            
                    _row.AttrCritDamageAdd = parseInt(line[10]);
            
                    _row.AttrHealIntensityAdd = parseInt(line[11]);
            
                    _row.AttrHealAdd = parseInt(line[12]);
            
                    _row.AttrHealedAdd = parseInt(line[13]);
            
                    _row.AttrAttackSpeedPCTAdd = parseInt(line[14]);
            
                    _row.AttrElementPenPCTAdd = parseInt(line[15]);
            
                    _row.AttrElementRedPCTAdd = parseInt(line[16]);
            
                    _row.AttrElementPhysicalPenPCTAdd = parseInt(line[17]);
            
                    _row.AttrElementPhysicalRedPCTAdd = parseInt(line[18]);
            
                    _row.AttrElementMagicPenPCTAdd = parseInt(line[19]);
            
                    _row.AttrElementMagicRedPCTAdd = parseInt(line[20]);
            
                    _row.AttrDmgAAdd = parseInt(line[21]);
            
                    _row.AttrDmgRedAdd = parseInt(line[22]);
            
                    _row.AttrSkillCDPCTAdd = parseInt(line[23]);
            
                }
            }
        }
    }
}