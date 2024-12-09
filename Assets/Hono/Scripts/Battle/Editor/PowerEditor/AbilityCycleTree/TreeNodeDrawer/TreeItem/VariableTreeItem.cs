using System;
using System.Collections.Generic;
using Editor.BattleEditor.AbilityEditor;
using Hono.Scripts.Battle;
using Hono.Scripts.Battle.Base;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class VariableTreeItem : ATreeItem<VariableNodeData>
    {
        public VariableTreeItem(AbilityCycleTree tree, ATreeEditorNode data) : base(tree, data)
        {
            ButtonBackGroundColor = new Color(0.6f, 0.3f, 0.95f);
        }


        protected override ERightMenuState checkRightMenuState(MenuInfo info)
        {
            throw new NotImplementedException();
        }

        protected override string getButtonText()
        {
            var parentData = parent.EditorNode.Data;
            string name ="未设置";

            if (parentData is ActionNodeData actionNode)
            {
                if (AbilityFunctionHelper.TryGetFuncInfo(actionNode.Function.funcName, out var funcInfo))
                {
                    if (funcInfo.ReturnType == typeof(void))
                    {
                        return "获取返回值失败函数没有返回值!!";
                    }

                    return "获取返回值 (name：" + name + ") 返回值类型：" + funcInfo.ReturnType;
                }

                return "获取函数失败";
            }

            return "设置变量 " + name + " = " + Data.Value;
        }
        
        protected override void OnBtnClicked(Rect btnRect)
        {
            ANodeSettingWindow.Open<VariableSettingWindow>(this);
        }
    }

    public class VariableSettingWindow : ANodeSettingWindow< VariableNodeData>
    {
        private ParameterField _value;

        private List<string> _dropList = new List<string>()
        {
            "int",
            "float",
            "bool",
            "string",
        };

        private string _curSelect;
        private string _customTypeStr;
        private bool _customCastSuccess;
        private bool _isGetReturn;

        protected override void Init()
        {
            _value = new ParameterField(TempData.Value, "变量值：",
               TempData.Value.GetType());
           
            if (_curSelect == "custom")
            {
                _customTypeStr = _curSelect;
                _customCastSuccess = Type.GetType(_customTypeStr) != null;
            }
            else
            {
                _customCastSuccess = true;
            }

            _isGetReturn = false;
        }

        protected override void Draw()
        {
            SirenixEditorGUI.BeginBox();

            /*
            TempData.Name = SirenixEditorFields.TextField("变量名：", TempData.Name);

            if (TempData.ParentId > 0)
            {
                var parentNode = AbilityViewDrawer.AbilityData.NodeDict[TempData.ParentId];
                if (parentNode.NodeType == EAbilityNodeType.EAction)
                {
                    var actionNode = (ActionNodeData)parentNode;
                    if (AbilityFunctionHelper.TryGetFuncInfo(actionNode.Function.funcName, out var funcInfo))
                    {
                        if (funcInfo.ReturnType == typeof(void))
                        {
                            _isGetReturn = false;
                        }

                        _curSelect = funcInfo.ReturnType.ToString();
                        _isGetReturn = true;
                    }
                }
            }
            
            _curSelect = SirenixEditorFields.Dropdown(new GUIContent("变量类型"), _curSelect, _dropList);
            if (_curSelect != TempData.typeString)
            {
	            TempData.typeString = _curSelect;
	            TempData.Value = new AParams();
                if (_curSelect != "custom")
                {
                    _value = new ParameterField(TempData.Value, "变量值：",
                        AbilityFunctionHelper.GetVariableType(TempData.typeString));
                }
                else
                {
                    EditorGUILayout.BeginHorizontal();
                    _customTypeStr = SirenixEditorFields.TextField("类型字符串", _customTypeStr);
                    if (SirenixEditorGUI.Button("转换检测", ButtonSizes.Medium))
                    {
                        var customType = Type.GetType(_customTypeStr);
                        _customCastSuccess = customType != null;
                        if (_customCastSuccess)
                        {
                            _value = new ParameterField(TempData.Value, "变量值：", customType);
                        }
                    }

                    EditorGUILayout.EndHorizontal();
                }
            }

            if (!_isGetReturn)
            {
                if (_customCastSuccess)
                {
                    _value.Draw();
                }
                else
                {
                    EditorGUILayout.LabelField("类型转换失败！");
                }
            }
            else {
	            TempData.typeString = _curSelect;
            }

            if (SirenixEditorGUI.Button("保   存", ButtonSizes.Large))
            {
                if (_customCastSuccess)
                {
                    Save();
                    AbilityViewDrawer.VarCollector.RefreshAllVariable();
                }
                else
                {
                    Debug.LogError("无法识别类型，无法保存");
                }
            }
            */

            SirenixEditorGUI.EndBox();
        }
    }
}