using System;
using Hono.Scripts.Battle.Tools;
using UnityEngine;

namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// 地图块更新原型
    /// </summary>
    public struct MotionInfo
    {
        public Vector2 GridIndex; 
        public Vector3 Pos; 
        public Vector3 Velocity; 
    }

    public readonly struct MotionInfoProxy
    {
        private MotionInfo[] ATransforms { get; }
        private int Index { get; }

        public MotionInfoProxy(MotionInfo[] transforms, int index)
        {
            ATransforms = transforms;
            Index = index;
        }

        public void SetPos(Vector3 v)
        {
            var span = new Span<MotionInfo>(ATransforms, Index, 1);
            span[0].Pos = v;
        }
    }

    public interface IAHandler
    {
        int GetProxyIndex();
    }

    public class A : Singleton<A>
    {
        private readonly MotionInfo[] _aTransforms = new MotionInfo[100];

        public MotionInfoProxy GetProxy(IAHandler handler)
        {
            return new MotionInfoProxy(_aTransforms, handler.GetProxyIndex());
        }

        public void Tick(float dt)
        {
            var proxy = new Span<MotionInfo>(_aTransforms);
            for (var index = 0; index < proxy.Length; index++)
            {
                var trans = proxy[index];
                trans.Pos += trans.Velocity * dt;
                trans.Velocity = Vector3.zero;
            }
        }
    }

    public class B : IAHandler
    {
        private int _idx;

        public B(int idx)
        {
            _idx = idx;
        }

        public void main()
        {
            A.Instance.GetProxy(this).SetPos(Vector3.down);
        }

        public int GetProxyIndex()
        {
            return _idx;
        }
    }
}