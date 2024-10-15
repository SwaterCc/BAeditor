using System;
using Hono.Scripts.Battle.Scene;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    [ExecuteInEditMode]
    public class SceneActorModel : ActorModel
    {
        private BattleLevelData _levelData;

        private void Awake()
        {
            if (ActorUid > 0) return;
            _levelData = FindObjectOfType<BattleLevelData>();
            if (_levelData == null)
            {
                Debug.LogError("��ʼ��SceneActorModel��Uidʧ�ܣ���ȷ�������н�����һ��BattleLevelDataʵ��");
                return;
            }

            ActorUid = _levelData.GetSceneActorUid();
            _levelData.SceneActorModels.Add(this);

            onInit();
        }

        protected virtual void onInit() { }

        public virtual void OnModelSetupFinish(Actor actor) { }

        protected virtual void onRemove() { }

        private void OnDestroy()
        {
            onRemove();
            if (_levelData != null)
            {
                _levelData.SceneActorModels.Remove(this);
            }
        }
    }
}