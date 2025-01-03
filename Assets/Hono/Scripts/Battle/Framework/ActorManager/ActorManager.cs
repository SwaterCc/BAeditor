#region

using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Tools;
using UnityEngine;
using UnityEngine.Profiling;

#endregion

namespace Hono.Scripts.Battle
{
    public partial class ActorManager : Singleton<ActorManager>,IBattleFrameworkTick
    {
        /// <summary>
        /// 正在运行的actor列表
        /// </summary>
        private readonly List<Actor> _runningActorList = new(2000);

        /// <summary>
        /// actor索引字典
        /// </summary>
        private readonly Dictionary<int, Actor> _uidActorDict = new(2000);

        /// <summary>
        /// 删除列表
        /// </summary>
        private readonly List<Actor> _removeList = new(100);

        public ActorManager()
        {
            _filter = new Filter(this);
        }

        public void ForeachActor(Action<Actor> action)
        {
            foreach (var item in _runningActorList)
            {
                switch (item.ActorType)
                {
                    case EActorType.Pawn:
                    case EActorType.Building:
                    case EActorType.Monster:
                        action(item);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 创建Actor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="configId"></param>
        /// <param name="actorModel"></param>
        /// <param name="afterSetupCallFunc"></param>
        /// <returns>返回Actor对象的Uid</returns>
        public int CreateActor(EActorType type, int configId = 0, ActorModel actorModel = null,
            Action<Actor> afterSetupCallFunc = null)
        {
            var actor = getActor(type, configId, actorModel, afterSetupCallFunc);
            actor.SetAttr(EAttrType.AttrSourceActorUid, actor.Uid, false);
            actor.SetAttr(EAttrType.AttrTopSourceActorUid, actor.Uid, false);
            return actor.Uid;
        }

        /// <summary>
        /// 召唤Actor
        /// </summary>
        /// <param name="summoner">召唤者</param>
        /// <param name="type">Actor类型，目前仅支持Bullet，HitBox，Monster，Building</param>
        /// <param name="configId">配置Id</param>
        /// <param name="fromTopSummer">是否属于最顶层召唤者</param>
        /// <param name="afterSetupCallFunc">Setup后的回调</param>
        /// <returns>返回Actor对象的Uid</returns>
        public int SummonActor(Actor summoner, EActorType type, int configId, bool fromTopSummer,
            Action<Actor> afterSetupCallFunc = null)
        {
            var summoned = getActor(type, configId, null, afterSetupCallFunc);
            summoned.SetAttr(EAttrType.AttrIsSummoned, 1, false);
            var sourceUid = fromTopSummer
                ? summoner.GetAttr(EAttrType.AttrTopSourceActorUid)
                : summoner.GetAttr(EAttrType.AttrSourceActorUid);
            summoned.SetAttr(EAttrType.AttrSourceActorUid, sourceUid, false);
            summoned.SetAttr(EAttrType.AttrTopSourceActorUid, summoner.GetAttr(EAttrType.AttrTopSourceActorUid),
                false);
            summoned.SetAttr(EAttrType.AttrFaction, summoner.GetAttr(EAttrType.AttrFaction), false);
            return summoned.Uid;
        }

        /// <summary>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="configId"></param>
        /// <param name="callback"></param>
        /// <param name="actorModel"></param>
        /// <returns></returns>
        private Actor getActor(in EActorType type, in int configId, in ActorModel actorModel, Action<Actor> callback)
        {
            Actor actor = APool<Actor>.Pool.Rent();
            bool res = false;
            int uid = 0;
            bool needGenerateUid = true;
            if (actorModel != null && actorModel.ActorUid is > 0 and < 10000)
            {
                uid = actorModel.ActorUid;
                needGenerateUid = false;
            }

            switch (type)
            {
                case EActorType.Pawn:
                case EActorType.Monster:
                case EActorType.Building:
                    uid = needGenerateUid ? ActorUidGenerator.GenerateUid(EActorUidRangeType.NormalActor) : uid;
                    actor.Init(uid, type);
                    res = MajorActorFactory.ActorSetup(ref actor, configId, actorModel);
                    break;
                case EActorType.Bullet:
                case EActorType.HitBox:
                case EActorType.Loot:
                    uid = needGenerateUid ? ActorUidGenerator.GenerateUid(EActorUidRangeType.DynamicActor) : uid;
                    actor.Init(uid, actor.ActorType);
                    res = DynamicActorFactory.ActorSetup(ref actor);
                    break;
                case EActorType.BattleLevelController:
                case EActorType.MonsterGenerator:
                case EActorType.TriggerBox:
                    if (uid <= 0)
                    {
                        Debug.LogError("场景对象必须指定Uid");
                        break;
                    }

                    actor.Init(uid, actor.ActorType);
                    res = SceneActorFactory.ActorSetup(ref actor, actorModel);
                    break;
            }

            if (!res)
            {
                Debug.LogError("创建Actor失败");
                APool<Actor>.Pool.Recycle(actor);
                return null;
            }

            callback?.Invoke(actor);

            addToScene(actor);

            return actor;
        }

        /// <summary>
        /// 创建后的对象添加到场景中
        /// </summary>
        /// <param name="actor"></param>
        private void addToScene(in Actor actor)
        {
            if (_uidActorDict.ContainsKey(actor.Uid))
            {
                Debug.LogError("uid 重复");
                return;
            }

            _runningActorList.Add(actor);
            _uidActorDict.Add(actor.Uid, actor);
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
            return _uidActorDict.GetValueOrDefault(uid);
        }

        public bool TryGetActor(int uid, out Actor actor)
        {
            return _uidActorDict.TryGetValue(uid, out actor);
        }

        public bool HasActor(int uid)
        {
            return _uidActorDict.ContainsKey(uid);
        }

        public void RemoveActor(int actorUid)
        {
            if (!_uidActorDict.TryGetValue(actorUid, out Actor actor))
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
            _uidActorDict.Clear();
            _removeList.Clear();
        }
    }
}