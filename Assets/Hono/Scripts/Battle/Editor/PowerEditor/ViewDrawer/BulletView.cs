using Editor.AbilityEditor;
using Editor.AbilityEditor.SimpleWindow;
using Hono.Scripts.Battle;
using Hono.Scripts.Battle.Editor.AbilityEditor;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.BattleEditor.AbilityEditor
{
    public class BulletView : AView<BulletData>
    {
        private bool _isBulletDrawerTab = true;
        private bool _isAbilityDrawerTab;

        private readonly AbilityView _abilityView = new();

        public override void Load(string path)
        {
            base.Load(path);
            _abilityView.Data = Data.BulletAbility;
        }

        protected override void onInit()
        {
            _abilityView.Init();
        }

        private void drawBullet()
        {
            SirenixEditorGUI.BeginBox("Bullet数据");
            PowerEditorUIHelper.DrawSimpleField(ref Data.hitRadius, "子弹有效半径", Data.hitRadius, true);
            PowerEditorUIHelper.DrawSimpleField(ref Data.ignoreAllNotTarget, "忽略所有不是target的对象", Data.ignoreAllNotTarget,
                                                true);
            if (!Data.ignoreAllNotTarget)
            {
                if (SirenixEditorGUI.Button("非目标对象碰撞条件", ButtonSizes.Large))
                {
                    SerializableOdinWindow.Open(Data.notTargetHitCondition);
                }

                PowerEditorUIHelper.DrawSimpleField(ref Data.minHitInterval, "最小命中间隔", Data.minHitInterval, true);
                PowerEditorUIHelper.DrawSimpleField(ref Data.maxHitCount,    "最大命中次数", Data.maxHitCount,    true);
            }

            PowerEditorUIHelper.DrawSimpleField(ref Data.speed,        "速度",          Data.speed,        true);
            PowerEditorUIHelper.DrawSimpleField(ref Data.acceleration, "加速度",         Data.acceleration, true);
            PowerEditorUIHelper.DrawSimpleField(ref Data.rotSpeed,     "转向速度(-1为秒转)", Data.rotSpeed,     true);
            PowerEditorUIHelper.DrawSimpleField(ref Data.lifeTime,     "子弹生命时长",      Data.lifeTime,     true);

            if (Data.isUseVFXKeyModel)
            {
                PowerEditorUIHelper.DrawSimpleField(ref Data.flyVFXStr, "飞行特效Key", Data.flyVFXStr, true);
                PowerEditorUIHelper.DrawSimpleField(ref Data.hitVFXStr, "子弹销毁时的特效Key", Data.hitVFXStr, true);
            }
            else
            {
                Data.flyVFXStr = PowerEditorUIHelper.DrawObjectField<GameObject>("飞行特效",     Data.flyVFXStr);
                Data.hitVFXStr = PowerEditorUIHelper.DrawObjectField<GameObject>("子弹销毁时的特效", Data.hitVFXStr);
            }
         
            SirenixEditorGUI.EndBox();
        }

        public override void Draw()
        {
            SirenixEditorGUI.BeginHorizontalToolbar();
            if (SirenixEditorGUI.ToolbarTab(_isBulletDrawerTab, "Bullet配置"))
            {
                _isBulletDrawerTab = true;
                _isAbilityDrawerTab = false;
            }

            if (SirenixEditorGUI.ToolbarTab(_isAbilityDrawerTab, "Ability"))
            {
                _isAbilityDrawerTab = true;
                _isBulletDrawerTab = false;
            }

            SirenixEditorGUI.EndHorizontalToolbar();

            if (_isBulletDrawerTab)
            {
                drawBullet();
            }

            if (_isAbilityDrawerTab)
            {
                _abilityView?.Draw();
            }
        }

        public class BulletViewDrawer : AViewDrawer<BulletView> { }
    }
}