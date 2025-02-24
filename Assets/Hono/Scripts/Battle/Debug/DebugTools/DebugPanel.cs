using Hono.Scripts.Battle.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hono.Scripts.Battle
{
    public class DebugPanel : MonoBehaviour
    {
        public InputField createInputField;
        public Button createButton;
        
        public InputField switchInputField;
        public Button switchButton;
        
        private bool _isListener;

        public void Start()
        {
            createButton.onClick.AddListener(OnCreateButtonClick);
            switchButton.onClick.AddListener(OnSwitchButtonClick);
        }

        private void OnCreateButtonClick()
        {
            _isListener = true;
        }

        private void OnSwitchButtonClick()
        {
            World.Current.SwitchPlayerControlUnit(int.Parse(switchInputField.text));
        }

        public void Update()
        {
            // 如果正在监听鼠标点击
            if (_isListener)
            {
                // 检测鼠标左键点击
                if (Input.GetMouseButtonDown(0))
                {
                    // 从摄像机发射射线
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    // 检测射线是否与地面碰撞
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        // 获取地面坐标
                        Vector3 groundPosition = hit.point;
                        for (int i = 0; i < 500; i++)
                        {
                            World.Current.CreateActor(createInputField.text, 1, groundPosition, Quaternion.identity);
                        }
                        
                        // 关闭监听
                        _isListener = false;
                    }
                }
            }
        }
    }
}