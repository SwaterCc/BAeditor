#region

using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Tools;
using UnityEngine;
using UnityEngine.Profiling;

#endregion

namespace Hono.Scripts.Battle
{
    public partial class ActorManager : Singleton<ActorManager>, IBattleFrameworkTick
    {
        /// <summary>
        /// 正在运行的actor列表(高频率更新)
        /// </summary>
        private readonly List<Actor> _runningActorList = new(2000);

        /// <summary>
        /// actor索引字典
        /// </summary>
        private readonly Dictionary<int, Actor> _actorSearch = new(2000);

        /// <summary>
        /// 删除列表
        /// </summary>
        private readonly List<Actor> _removeList = new(100);

        /// <summary>
        /// 对象工厂
        /// </summary>
        private readonly ActorFactory _factory = new();
        public static ActorFactory Factory => Instance._factory;

        /// <summary>
        /// 创建Actor
        /// </summary>
        public Actor CreateActor(int actorTableId = 0, List<AttrSnapshot> attrs = null)
        {
            Actor actor = APool<Actor>.Pool.Rent();
            var uid = ActorUidGenerator.GenerateUid(EActorUidRangeType.NormalActor);
            actor.Init(uid, actorTableId, null, attrs);
            return actor;
        }

        /// <summary>
        /// 召唤Actor
        /// </summary>
        public Actor SummonActor(Actor summoner, int actorTableId, string rule, bool fromTopSummer)
        {
            Actor summoned = APool<Actor>.Pool.Rent();
            var uid = ActorUidGenerator.GenerateUid(EActorUidRangeType.NormalActor);
            summoned.Init(uid, actorTableId, null, summoner.Attrs.GetAttrSnapShots(rule));
            summoned.SetAttr(EAttrType.AttrIsSummoned, 1);
            var sourceUid = fromTopSummer
                ? summoner.GetAttr(EAttrType.AttrTopSourceActorUid)
                : summoner.GetAttr(EAttrType.AttrSourceActorUid);
            summoned.SetAttr(EAttrType.AttrSourceActorUid,    sourceUid);
            summoned.SetAttr(EAttrType.AttrTopSourceActorUid, summoner.GetAttr(EAttrType.AttrTopSourceActorUid));
            summoned.SetAttr(EAttrType.AttrFaction,           summoner.GetAttr(EAttrType.AttrFaction));

            return summoned;
        }

        /// <summary>
        /// 创建打击盒子
        /// </summary>
        public Actor CreateHitBox(Actor attacker)
        {
            return null;
        }

        /// <summary>
        /// 创建子弹
        /// </summary>
        public Actor CreateBullet()
        {
            return null;
        }


        /// <summary>
        /// 创建后的对象添加到场景中
        /// </summary>
        /// <param name="actor"></param>
        private void addToScene(Actor actor)
        {
            if (_actorSearch.ContainsKey(actor.Uid))
            {
                Debug.LogError("uid 重复");
                return;
            }

            _runningActorList.Add(actor);
            _actorSearch.Add(actor.Uid, actor);
            actor.EnterScene();
        }

        public void Tick(float dt)
        {
            Profiler.BeginSample("AllActorTick");

            int i = 0;
            while (i < _runningActorList.Count)
            {
                _runningActorList[i++].Tick(dt);
            }


            if (_removeList.Count != 0)
            {
                foreach (var actor in _removeList)
                {
                    actor.ExitScene();
                    _runningActorList.RemoveSwapBack(actor);
                    APool<Actor>.Pool.Recycle(actor);
                }

                _removeList.Clear();
            }

            Profiler.EndSample();
        }

        public Actor GetActor(int uid)
        {
            return _actorSearch.GetValueOrDefault(uid);
        }

        public bool TryGetActor(int uid, out Actor actor)
        {
            return _actorSearch.TryGetValue(uid, out actor);
        }

        public bool HasActor(int uid)
        {
            return _actorSearch.ContainsKey(uid);
        }

        public void RemoveActor(int actorUid)
        {
            if (!_actorSearch.TryGetValue(actorUid, out Actor actor))
            {
                return;
            }

            actor.ExitScene();
            _removeList.Add(actor);
        }

        public void ClearAllActor()
        {
            foreach (Actor actor in _runningActorList)
            {
                actor.ExitScene();
                APool<Actor>.Pool.Recycle(actor);
            }

            _runningActorList.Clear();
            _actorSearch.Clear();
            _removeList.Clear();
        }
    }
}