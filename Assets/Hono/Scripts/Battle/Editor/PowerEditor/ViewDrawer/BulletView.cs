using Editor.AbilityEditor;
using Editor.AbilityEditor.SimpleWindow;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.BattleEditor.AbilityEditor
{
    public class BulletView: AView<BulletData>
    {
        public override void Draw()
        {
            SirenixEditorGUI.BeginBox("Buff数据");
            /*Data.CustomMotion = EditorGUILayout.Toggle(new GUIContent("自定义位移(默认使用直线位移)"), Data.CustomMotion);
            Data.CloseFollowTarget = EditorGUILayout.Toggle(new GUIContent("关闭追踪目标"), Data.CloseFollowTarget);
            Data.BulletSpeed = SirenixEditorFields.FloatField("子弹速度",Data.BulletSpeed);
            //_data.Offset = SirenixEditorFields.Vector3Field("子弹相对于召唤者的偏移", _data.Offset);
            Data.IsHitPathActor = EditorGUILayout.Toggle(new GUIContent("是否命中路径上的Actor"), Data.IsHitPathActor);
            Data.DamageConfigId = SirenixEditorFields.IntField("伤害配置Id",Data.DamageConfigId);
            Data.BulletLifeTime = SirenixEditorFields.FloatField("子弹存在时长",Data.BulletLifeTime);
            Data.MaxHitCount = SirenixEditorFields.IntField("子弹最大命中数量(如果关闭路径命中，则命中次数不会增长)",Data.MaxHitCount);

            /*if (SirenixEditorGUI.Button("配置子弹命中筛选器",ButtonSizes.Medium))
            {
                FilterSettingWindow.Open(ref _data.rangeFilterSetting);
            }#1#*/
            SirenixEditorGUI.EndBox();
        }
        
        public class BulletViewDrawer : AViewDrawer<BulletView> { }
    }
}