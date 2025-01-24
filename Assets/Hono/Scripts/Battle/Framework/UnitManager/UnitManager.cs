#region

using System.Collections.Generic;
using Hono.Scripts.Battle.Core;
using Hono.Scripts.Battle.Tools;
using UnityEngine;
using UnityEngine.Profiling;

#endregion

namespace Hono.Scripts.Battle
{
    /// <summary>
    /// 管理Unit生命周期，还有Tick频率
    /// </summary>
    public class UnitManager : Singleton<UnitManager>, IBattleFrameworkTick
    {
        /// <summary>
        /// 正在运行的actor列表(高频率更新)
        /// </summary>
        private readonly List<Actor> _rootActorList = new(1000);
        
        /// <summary>
        /// 删除列表
        /// </summary>
        private readonly List<Unit> _removeList = new(400);
        
       
        /// <summary>
        /// 创建后的对象添加到场景中
        /// </summary>
        /// <param name="unit"></param>
        private void addToWorld(Unit unit)
        {
            if (ActorSearch.ContainsKey(unit.Uid))
            {
                Debug.LogError("uid 重复");

                return;
            }

            _rootActorList.Add(actor);
            ActorSearch.Add(actor.Uid, actor);
            actor.EnterScene();
        }

        public void Tick(float dt)
        {
            Profiler.BeginSample("AllUnitTick");

            int i = 0;
            while (i < _rootActorList.Count)
            {
                _rootActorList[i++].Tick(dt);
            }


            if (_removeList.Count != 0)
            {
                foreach (var actor in _removeList)
                {
                    actor.ExitScene();
                    _rootActorList.RemoveSwapBack(actor);
                    APool<Actor>.Pool.Recycle(actor);
                }

                _removeList.Clear();
            }

            Profiler.EndSample();
        }

        public Actor GetActor(int uid)
        {
            return ActorSearch.GetValueOrDefault(uid);
        }

        public bool TryGetActor(int uid, out Actor actor)
        {
            return ActorSearch.TryGetValue(uid, out actor);
        }

        public bool HasActor(int uid)
        {
            return ActorSearch.ContainsKey(uid);
        }

        public void RemoveActor(int actorUid)
        {
            if (!ActorSearch.TryGetValue(actorUid, out Actor actor))
            {
                return;
            }

            actor.ExitScene();
            _removeList.Add(actor);
        }

        public void ClearAllActor()
        {
            foreach (Actor actor in _rootActorList)
            {
                actor.ExitScene();
                APool<Actor>.Pool.Recycle(actor);
            }

            _rootActorList.Clear();
            ActorSearch.Clear();
            _removeList.Clear();
        }
    }
}