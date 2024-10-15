using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;


namespace Hono.Scripts.Battle.Scene
{
    public class BattleLevelData : MonoBehaviour
    {
        #region 场景数据

        [LabelText("场景对象")]
        public List<SceneActorModel> SceneActorModels = new();
        
        [LabelText("失败后可重复挑战")]
        public bool CanRepeatRound;
        
        [LabelText("波次信息")]
        [ListDrawerSettings(ShowFoldout = true)]
        public List<RoundData> RoundDatas = new();


        #endregion


        [SerializeField]
        [ShowInInspector]
        [ReadOnly]
        [LabelText("下一个Uid")]
        private int _sceneUidCounter;

        public int GetSceneActorUid()
        {
            if(_sceneUidCounter is < 1000 or > 5000 )
            {
                //超出了Id范围,重置
                _sceneUidCounter = 1000;
            }

            ++_sceneUidCounter;
            return _sceneUidCounter;
        }

        public void Awake()
        {
           var dataObjects = FindObjectsOfType<BattleLevelData>();
           if (dataObjects.Length > 1)
           {
               Debug.LogError("场景中仅能存在一个BattleLevelData对象！已删除自身");
               Destroy(this);
           }
        }
    }
}