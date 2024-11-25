#region

using System;
using Hono.Scripts.Battle.Tools.DebugTools;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    public class TeamRefreshPoint : ActorModel
    {
        [NonSerialized] public bool IsBuildingModel;
        [NonSerialized] public int TeamId;

        private bool _canBuilding;
        private ActorModelFollowMouse _followMouse;

        private void Awake()
        {
            //ActorType = EActorType.TeamRefreshPoint;
        }

        private void Start()
        {
            if (_followMouse != null)
            {
                return;
            }

            if (!TryGetComponent(out _followMouse))
            {
                _followMouse = gameObject.AddComponent<ActorModelFollowMouse>();
            }

            _followMouse.enabled = IsBuildingModel;
        }


        public void ChangeBuildState(bool canBuild)
        {
            _canBuilding = canBuild;
        }

        public void Update()
        {
            if (!IsBuildingModel) return;

            if (Input.GetMouseButtonDown(0) && _canBuilding)
            {
                //左键
                IsBuildingModel = false;
                _followMouse.enabled = false;

                /*ActorManager.Instance.CreateActor(EActorType.TeamRefreshPoint, 0, (point) => {
                    point.SetAttr(ELogicAttr.AttrPosition, transform.position, false);
                    BattleManager.CurBattle.AddTeamRefreshPoint(TeamId, point.Uid);
                });*/
                Destroy(gameObject);
            }

            if (Input.GetMouseButtonDown(1))
            {
                _followMouse.enabled = false;
                Destroy(gameObject);
            }
        }
    }
}