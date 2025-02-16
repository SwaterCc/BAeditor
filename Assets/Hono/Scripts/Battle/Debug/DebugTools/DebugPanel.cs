using Hono.Scripts.Battle.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hono.Scripts.Battle
{
    public class DebugPanel : MonoBehaviour
    {
        public InputField inputField;
        public Button button;
        private bool _isListener;

        public void Start()
        {
            button.onClick.AddListener(OnCreateButtonClick);
        }

        private void OnCreateButtonClick()
        {
            _isListener = true;
           
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
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 8))
                    {
                        // 获取地面坐标
                        Vector3 groundPosition = hit.point;
                        World.Current.CreateActor(inputField.text, groundPosition,Quaternion.identity);

                        // 关闭监听
                        _isListener = false;
                    }
                }
            }
        }
    }
}