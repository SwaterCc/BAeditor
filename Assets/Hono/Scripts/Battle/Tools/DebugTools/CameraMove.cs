using System;
using UnityEngine;

namespace Hono.Scripts.Battle.Tools.DebugTools
{
    public class CameraMove : MonoBehaviour
    {
        public GameObject followActor;

        private Vector3 originPos;
        
        public void Awake()
        {
            originPos = transform.position;
        }

        public void Update()
        {
            if(followActor == null) return;
            transform.localPosition = originPos + followActor.transform.position;
        }
    }
}