using System.Collections.Generic;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public interface IWindowInit {
    		public void Init(AbilityNodeData nodeData);
    		public Rect GetPos();
    		public GUIContent GetWindowName();
    	}
    
    	public abstract class BaseNodeWindow<T> : EditorWindow where T : EditorWindow, IWindowInit {
    		public static EditorWindow GetWindow(AbilityNodeData nodeData) {
    			var window = GetWindow<T>();
    			window.position = window.GetPos();
    			window.titleContent = window.GetWindowName();
    			window.Init(nodeData);
    			return window;
    		}
    
    		public AbilityNodeData NodeData;
    		public Stack<EditorWindow> WindowStack = new Stack<EditorWindow>();
    
    		public void Init(AbilityNodeData nodeData) {
    			NodeData = nodeData;
    			onInit();
    		}
    
    		protected abstract void onInit();
    
    		public virtual Rect GetPos() {
    			return GUIHelper.GetEditorWindowRect().AlignCenter(740, 600);
    		}
    
    		public virtual GUIContent GetWindowName() {
    			return this.titleContent;
    		}
    
    		private void OnDestroy() {
    			foreach (var window in WindowStack) {
    				window.Close();
    			}
    
    			WindowStack.Clear();
    		}
    	}
    
    	public abstract class BaseNodeOdinWindow<T> : OdinEditorWindow where T : OdinEditorWindow, IWindowInit {
    		public static T GetWindow(AbilityNodeData nodeData) {
    			var window = GetWindow<T>();
    			window.position = window.GetPos();
    			window.titleContent = window.GetWindowName();
    			window.Init(nodeData);
    			return window;
    		}
    
    		[HideInInspector] public AbilityNodeData NodeData;
    
    		public void Init(AbilityNodeData nodeData) {
    			NodeData = nodeData;
    			onInit();
    		}
    
    		protected abstract void onInit();
    
    		public virtual Rect GetPos() {
    			return GUIHelper.GetEditorWindowRect().AlignCenter(400, 600);
    		}
    
    		public virtual GUIContent GetWindowName() {
    			return this.titleContent;
    		}
    	}
}