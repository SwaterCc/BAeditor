using Editor.AbilityEditor;
using Editor.AbilityEditor.SimpleWindow;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.BattleEditor.AbilityEditor
{
    public class BulletDrawer: IExDrawer
    {
        private BulletData _data;
        public void LoadAsset(int id)
        {
            string path = AbilityEditorPath.BulletPath + "/" + id + ".asset";
            _data = AssetDatabase.LoadAssetAtPath<BulletData>(path);
            if (_data == null)
            {
                _data = ScriptableObject.CreateInstance<BulletData>();
                _data.Id = id;

                AssetDatabase.CreateAsset(_data, path);
            }
        }

        public void Draw()
        {
            SirenixEditorGUI.BeginBox("Buff数据");
            _data.CustomMotion = EditorGUILayout.Toggle(new GUIContent("自定义位移(默认使用直线位移)"), _data.CustomMotion);
            _data.CloseFollowTarget = EditorGUILayout.Toggle(new GUIContent("关闭追踪目标"), _data.CloseFollowTarget);
            _data.BulletSpeed = SirenixEditorFields.FloatField("子弹速度",_data.BulletSpeed);
            _data.Offset = SirenixEditorFields.Vector3Field("子弹相对于召唤者的偏移", _data.Offset);
            _data.IsHitPathActor = EditorGUILayout.Toggle(new GUIContent("是否命中路径上的Actor"), _data.IsHitPathActor);
            _data.DamageConfigId = SirenixEditorFields.IntField("伤害配置Id",_data.DamageConfigId);
            _data.BulletLifeTime = SirenixEditorFields.FloatField("子弹存在时长",_data.BulletLifeTime);
            _data.MaxHitCount = SirenixEditorFields.IntField("子弹最大命中数量(如果关闭路径命中，则命中次数不会增长)",_data.MaxHitCount);

            if (SirenixEditorGUI.Button("配置子弹命中筛选器",ButtonSizes.Medium))
            {
                FilterSettingWindow.Open(ref _data.FilterSetting);
            }
            SirenixEditorGUI.EndBox();
        }

        public void Save()
        {
            if (!_data) return;
            EditorUtility.SetDirty(_data);
            AssetDatabase.SaveAssets();
        }
    }
}