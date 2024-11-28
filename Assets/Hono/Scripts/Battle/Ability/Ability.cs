#region

using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Base;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    /// <summary>
    /// 这个Ability代表了运行时流程管理
    /// </summary>
    public sealed partial class Ability : IAPoolObject
    {
        /// <summary>
        /// 基础数据配置Id
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 该能力属于哪个Actor
        /// </summary>
        public Actor Actor { get; private set; }

        /// <summary>
        /// Ability数据
        /// </summary>
        public AbilityData Data { get; private set; }

        /// <summary>
        /// 属于Ability的变量
        /// </summary>
        public VarCollection Vairables { get; }

        /// <summary>
        /// tags
        /// </summary>
        public TagCollection TagCollection { get; }

        /// <summary>
        /// 周期管控
        /// </summary>
        private readonly AbilityCycle _abilityCycle;

        /// <summary>
        /// 指令缓存
        /// </summary>
        private readonly HashSet<ICommand> _commandCaches;

        /// <summary>
        /// 逻辑帧时间缩放系数
        /// </summary>
        public float TimeScaleFactory { get; set; }

        public Ability()
        {
            _abilityCycle = new AbilityCycle(this);
            _commandCaches = new HashSet<ICommand>(20);
            Vairables = new VarCollection(10);
            TagCollection = new TagCollection();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="configId"></param>
        /// <returns></returns>
        public bool Init(in Actor actor, in int configId)
        {
            Actor = actor;

            Data = AssetManager.Instance.GetData<AbilityData>(configId);
            if (Data == null)
            {
                Debug.LogError($"加载Ability {configId} 失败");
                return false;
            }

            Id = Data.id;
            TimeScaleFactory = 1;
            Vairables.SetParent(actor.Variables);
            TagCollection.SetParent(actor.TagCollection);
            foreach (var tag in Data.Tags)
            {
                TagCollection.Add(tag);
            }

            _abilityCycle.Init();
            return true;
        }

        /// <summary>
        /// 运行，当帧执行
        /// </summary>
        public void Execute()
        {
            _abilityCycle.Execute();
        }

        /// <summary>
        /// 停止，当帧执行
        /// </summary>
        public void Stop()
        {
            _abilityCycle.Stop();
        }

        /// <summary>
        /// 重加载，Debug模式下运行
        /// </summary>
        public void Reload()
        {
            //终止能力运行
            _abilityCycle.Stop();

            //清理变量
            Vairables.Clear();

            //指令撤销
            foreach (var command in _commandCaches)
            {
                command.Undo();
            }

            //重新加载数据
            Data = AssetManager.Instance.GetData<AbilityData>(Id);
            if (Data == null)
            {
                Debug.LogError($"Reload Ability {Id} 失败");
                return;
            }

            //重新加载
            _abilityCycle.Init();
        }

        public void OnTick(float dt)
        {
            float timeFactory = Mathf.Clamp(TimeScaleFactory, 0, 5);
            _abilityCycle.Tick(dt * timeFactory);
        }

        /// <summary>
        /// 添加回调
        /// </summary>
        /// <param name="cycle"></param>
        /// <param name="callback"></param>
        public void AddCycleCallback(EAbilityCycle cycle, Action callback)
        {
            _abilityCycle.CycleCallbacks[cycle] += callback;
        }

        /// <summary>
        /// 清除回调
        /// </summary>
        /// <param name="cycle"></param>
        /// <param name="callback"></param>
        public void RemoveCycleCallback(EAbilityCycle cycle, Action callback)
        {
            _abilityCycle.CycleCallbacks[cycle] -= callback;
        }

        /// <summary>
        /// 添加指令
        /// </summary>
        /// <param name="command"></param>
        public void AddCommand(ICommand command)
        {
            command.Do();
            _commandCaches.Add(command);
        }

        /// <summary>
        /// 移除指令
        /// </summary>
        /// <param name="command"></param>
        public void RemoveCommand(ICommand command)
        {
            if (_commandCaches.Remove(command))
            {
                command.Undo();
            }
        }

        public void OnRecycle()
        {
            //指令回撤
            foreach (var command in _commandCaches)
            {
                command.Undo();
            }

            _commandCaches.Clear();

            //数据重置
            Id = 0;
            Actor = null;
            Data = null;
            //重置时间系数
            TimeScaleFactory = 1;

            _abilityCycle.Stop();
            _abilityCycle.OnRecycle();

            TagCollection.SetParent(null);
            TagCollection.Clear();

            Vairables.SetParent(null);
            Vairables.Clear();
        }
    }

    public static class AbilityDebug
    {
        private static string AbilityInfo(this Ability ability)
        {
            return $"[Ability] Id:{ability.Id} ActorUid:{ability.Actor.Uid} ";
        }

        public static void Log(this Ability ability, in string pattern, params object[] args)
        {
#if _ABILITY_DEBUG_
			Debug.Log(string.Format(AbilityInfo(ability) + pattern, args));
#endif
        }

        public static void LogError(this Ability ability, in string pattern, params object[] args)
        {
#if _ABILITY_DEBUG_
			Debug.LogError(string.Format(AbilityInfo(ability) + pattern, args));
#endif
        }
    }
}