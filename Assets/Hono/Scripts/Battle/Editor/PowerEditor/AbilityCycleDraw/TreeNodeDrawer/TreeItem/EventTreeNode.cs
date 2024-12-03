using System;
using System.Collections.Generic;
using Editor.BattleEditor.AbilityEditor;
using Hono.Scripts.Battle;
using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Tools.CustomAttribute;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem {
	public class EventTreeNode : ATreeNode {
		private new EventNodeData _nodeData;

		public EventTreeNode(AbilityCycleTree tree, AbilityNodeData nodeData) : base(tree, nodeData) {
			_nodeData = (EventNodeData)base._nodeData;
		}

		protected override void buildMenu() {
			_menu.AddItem(new GUIContent("创建节点/添加Action"), false,
				AddChild, (EAbilityNodeType.EAction));
			_menu.AddItem(new GUIContent("创建节点/添加If"), false,
				AddChild, (EAbilityNodeType.EBranchControl));
			_menu.AddItem(new GUIContent("创建节点/Set变量"), false,
				AddChild, (EAbilityNodeType.EVariableSetter));
			_menu.AddItem(new GUIContent("创建节点/SetAttr"), false,
				AddChild, (EAbilityNodeType.EAttrSetter));

			if (!checkHasParent(EAbilityNodeType.ERepeat)) {
				_menu.AddItem(new GUIContent("创建节点/创建Repeat节点"), false,
					AddChild, (EAbilityNodeType.ERepeat));
			}

			if (!checkHasParent(EAbilityNodeType.EGroup)) {
				_menu.AddItem(new GUIContent("创建节点/创建Stage节点"), false,
					AddChild, (EAbilityNodeType.EGroup));
			}
			
			_menu.AddItem(new GUIContent("重置"),false,() => {
				_nodeData.EventType = EBattleEventType.NoInit;
				_nodeData.CreateChecker = new AParams();
			});

			_menu.AddItem(new GUIContent("删除"), false,
				Remove);
		}

		protected override Color getButtonColor() {
			return new Color(0.4f, 0.8f, 0.5f);
		}

		protected override string getButtonText() {
			string text = "";
			if (_nodeData.IsEvent) {
				text = "等待事件：" + Enum.GetName(typeof(EBattleEventType), _nodeData.EventType);
			}
			else {
				var msgName = string.IsNullOrEmpty(_nodeData.MsgName) ? "未定义" : _nodeData.MsgName;
				text = "等待消息：" + msgName;
			}

			return text;
		}

		protected override string getButtonTips() {
			return "监听指定事件或消息回调，与执行顺序无关";
		}

		protected override void OnBtnClicked(Rect btnRect) {
			SettingWindow = BaseNodeWindow<EventNodeDataWindow, EventNodeData>.GetSettingWindow(_tree.TreeData,
				_nodeData,
				(nodeData) => {
					_tree.TreeData.NodeDict[nodeData.NodeId] = nodeData;
					_nodeData = nodeData;
				});
			SettingWindow.position = new Rect(btnRect.x, btnRect.y, 740, 240);
			SettingWindow.Show();
		}
	}

	public class EventNodeDataWindow : BaseNodeWindow<EventNodeDataWindow, EventNodeData>,
		IAbilityNodeWindow<EventNodeData> {
		private List<ParameterField> _parameterFields;
		private EBattleEventType _curEvent;

		protected override void onInit() {
			_parameterFields = new List<ParameterField>();
			_curEvent = _nodeData.EventType;
			
			if (!AbilityFunctionHelper.EventCheckerDict.TryGetValue(_nodeData.EventType, out var value)) {
				return;
			}

			if (!AbilityFunctionHelper.TryGetFuncInfo(value.CreateFuncName, out var funcInfo)) {
				return;
			}

			if (!string.IsNullOrEmpty(_nodeData.CreateChecker.funcName)) {
				for (int index = 0; index < _nodeData.CreateChecker.funcParams.Count; index++) {
					AParams aParameter = _nodeData.CreateChecker.funcParams[index];
					if(funcInfo.ParamInfos.Count <= index) continue;
					_parameterFields.Add(new ParameterField(aParameter, funcInfo.ParamInfos[index].ParamName,
						funcInfo.ParamInfos[index].ParamType));
				}
			}
		}

		private void initParameter() {
			if (!AbilityFunctionHelper.EventCheckerDict.TryGetValue(_nodeData.EventType, out var value)) {
				return;
			}

			if (!AbilityFunctionHelper.TryGetFuncInfo(value.CreateFuncName, out var funcInfo)) {
				return;
			}

			_nodeData.CreateChecker.paramType = EParamType.Function;
			_nodeData.CreateChecker.funcName = value.CreateFuncName;
			_nodeData.CreateChecker.funcParams ??= new List<AParams>();
			_nodeData.CreateChecker.funcParams.Clear();
			_parameterFields.Clear();
			foreach (var paramInfo in funcInfo.ParamInfos) {
				var parameter = new AParams();
				_nodeData.CreateChecker.funcParams.Add(parameter);
				var param = new ParameterField(parameter, paramInfo.ParamName, paramInfo.ParamType);
				_parameterFields.Add(param);
			}
		}

		private void OnGUI() {
			SirenixEditorGUI.BeginBox();

			_nodeData.IsEvent = SirenixEditorFields.Dropdown(new GUIContent("选择类型："), _nodeData.IsEvent,
				new[] { true, false },
				new[] { "事件", "消息" });

			if (_nodeData.IsEvent) {
				showEvent();
			}
			else {
				showMsg();
			}

			SirenixEditorGUI.EndBox();
		}

		private void showMsg() {
			_nodeData.MsgName = SirenixEditorFields.TextField("消息Key：", _nodeData.MsgName);
			if (SirenixEditorGUI.Button("保  存", ButtonSizes.Medium)) {
				Save();
			}
		}

		private void showEvent() {
			_nodeData.EventType = SirenixEditorFields.Dropdown(new GUIContent("事件类型"),
				_nodeData.EventType, AbilityFunctionHelper.AllowEvent);

			if (_curEvent != _nodeData.EventType) {
				initParameter();
				_curEvent = _nodeData.EventType;
			}

			if (_curEvent == EBattleEventType.NoInit || string.IsNullOrEmpty(_nodeData.CreateChecker.funcName)) {
				EditorGUILayout.LabelField("未初始化，请选择事件类型");
			}
			else {
				SirenixEditorGUI.BeginBox("参数设置");

				EditorGUILayout.BeginVertical();
					
				foreach (var parameterField in _parameterFields)
				{
					parameterField.Draw();
				}

				_nodeData.Desc = SirenixEditorFields.TextField("备注", _nodeData.Desc);

				if (SirenixEditorGUI.Button("保  存", ButtonSizes.Medium)) {
					Save();
				}

				EditorGUILayout.EndVertical();
				SirenixEditorGUI.EndBox();
			}
		}
	}
}