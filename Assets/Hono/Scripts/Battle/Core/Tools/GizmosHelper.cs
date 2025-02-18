#region

using System.Collections.Generic;
using Hono.Scripts.Battle.Core.Base;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.Tools
{
    public enum EGizmosDrawType
    {
        Cube,
        Sphere,
    }

    public class GizmosInfo
    {
        private float _duration = 0;
        public bool Invalid { get; private set; }

        public EGizmosDrawType DrawType;

        public Vector3 CubeSize;
        public Vector3 CenterPos;
        public Vector3 CubeTransform;
        public Quaternion Rot;
        public Color DrawColor;
        public float Radius;

        public float MaxDrawDuration = 2;

        public void Update(float dt)
        {
            _duration += dt;
            if (_duration >= MaxDrawDuration)
            {
                Invalid = true;
            }
        }
    }

    public class GizmosHelper : MonoSingleton<GizmosHelper>
    {
        private List<GizmosInfo> _drawList = new();
        private List<GizmosInfo> _removeList = new();

        public void DrawCube(Vector3 centerPos, Vector3 cubeSize, Quaternion rot, Color color)
        {
            var cube = new GizmosInfo()
            {
                DrawType = EGizmosDrawType.Cube,
                CubeSize = cubeSize,
                Rot = rot,
                CenterPos = centerPos,
                DrawColor = color
            };
            _drawList.Add(cube);
        }

        public void DrawSphere(Vector3 centerPos, float radius, Quaternion rot, Color color)
        {
            var sphere = new GizmosInfo()
            {
                DrawType = EGizmosDrawType.Sphere,
                Radius = radius,
                Rot = rot,
                CenterPos = centerPos,
                DrawColor = color
            };
            _drawList.Add(sphere);
        }

        public void DrawCapsule(Vector3 point1, Vector3 point2, float radius, Quaternion rotation, Color color)
        {
            Gizmos.color = color;

            // 计算胶囊体的方向向量
            Vector3 direction = (point2 - point1).normalized;
            float height = Vector3.Distance(point1, point2);

            // 绘制两端的球体
            Gizmos.DrawSphere(point1, radius);
            Gizmos.DrawSphere(point2, radius);

            // 绘制中间的圆柱体
            DrawCylinder(point1 + direction * radius, point2 - direction * radius, radius);
        }

        private void DrawCylinder(Vector3 start, Vector3 end, float radius)
        {
            int segments = 16; // 圆柱体的分段数
            Vector3 direction = (end - start).normalized;
            float height = Vector3.Distance(start, end);

            // 构造正交基
            Vector3 up = direction;
            Vector3 forward = Vector3.Cross(up,    Vector3.right).normalized;
            Vector3 right = Vector3.Cross(forward, up).normalized;

            // 绘制顶面和底面的圆
            for (int i = 0; i < segments; i++)
            {
                float angle = i * Mathf.PI * 2f / segments;
                float nextAngle = (i + 1) * Mathf.PI * 2f / segments;

                Vector3 offset1 = Mathf.Cos(angle) * right + Mathf.Sin(angle) * forward;
                Vector3 offset2 = Mathf.Cos(nextAngle) * right + Mathf.Sin(nextAngle) * forward;

                // 顶面
                Gizmos.DrawLine(start + offset1 * radius, start + offset2 * radius);
                // 底面
                Gizmos.DrawLine(end + offset1 * radius, end + offset2 * radius);

                // 侧面
                Gizmos.DrawLine(start + offset1 * radius, end + offset1 * radius);
            }
        }
        
        private void drawCube(GizmosInfo gizmosInfo)
        {
            if (gizmosInfo.Rot.w == 0) return;

            //设置颜色
            Gizmos.color = gizmosInfo.DrawColor;

            // 设置 Gizmos 矩阵，使矩形根据位置和尺寸绘制
            Gizmos.matrix = Matrix4x4.TRS(gizmosInfo.CenterPos, gizmosInfo.Rot, Vector3.one);

            // 绘制填充矩形
            Gizmos.DrawCube(Vector3.zero, gizmosInfo.CubeSize);
        }

        private void drawSphere(GizmosInfo gizmosInfo)
        {
            //设置颜色
            Gizmos.color = gizmosInfo.DrawColor;

            // 设置 Gizmos 矩阵，使矩形根据位置和尺寸绘制
            Gizmos.matrix = Matrix4x4.TRS(gizmosInfo.CenterPos, Quaternion.identity, Vector3.one);

            // 绘制填充矩形
            Gizmos.DrawSphere(Vector3.zero, gizmosInfo.Radius);
        }


        private void OnDrawGizmos()
        {
            foreach (var gizmosInfo in _drawList)
            {
                Matrix4x4 oldMatrix = Gizmos.matrix;
                switch (gizmosInfo.DrawType)
                {
                    case EGizmosDrawType.Cube:
                        drawCube(gizmosInfo);
                        break;
                    case EGizmosDrawType.Sphere:
                        drawSphere(gizmosInfo);
                        break;
                }

                Gizmos.matrix = oldMatrix;
                gizmosInfo.Update(Time.deltaTime);
                if (gizmosInfo.Invalid)
                {
                    _removeList.Add(gizmosInfo);
                }
            }

            foreach (var remove in _removeList)
            {
                _drawList.Remove(remove);
            }
        }
    }
}