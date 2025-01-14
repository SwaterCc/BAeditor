#region

using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Base;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    /// <summary>
    /// Actor的逻辑，包含逻辑层自身的逻辑和关联组件，最终决定出当前Actor逻辑层的属性
    /// </summary>
    public abstract partial class ActorLogic
    {
        public Actor Self { get; private set; }

        /// <summary>
        /// Actor的UID
        /// </summary>
        public int Uid => Self.Uid;

        /// <summary>
        /// Actor的黑板数据
        /// </summary>
        protected VariableBoard Variables => Self.VariableBoard;

        /// <summary>
        /// 输入来源
        /// </summary>
        private ActorInput _actorInput;

        /// <summary>
        /// 逻辑组件
        /// </summary>
        private readonly Dictionary<Type, AComponent> _components;

        /// <summary>
        /// ActionSystem
        /// </summary>
        
        protected ActorLogic()
        {
            _actorInput = new NoInput(this);
            _components = new Dictionary<Type, AComponent>(10);
        }

        public void OnSetup(Actor actor)
        {
            Self = actor;
            setupAttrs();
            foreach (var component in _components)
            {
                component.Value.Init();
            }

            onInit();
        }

        /// <summary>
        /// 重设Input
        /// </summary>
        /// <param name="input"></param>
        protected void resetInput(ActorInput input)
        {
            _actorInput = input;
        }

        /// <summary>
        /// 装载属性，先于OnInit
        /// </summary>
        protected virtual void setupAttrs() { }

        /// <summary>
        /// 在属性，状态机，组件初始化完成后调用
        /// </summary>
        protected virtual void onInit() { }

        /// <summary>
        /// 进入场景时调用
        /// </summary>
        public void EnterScene()
        {
            foreach (var component in _components)
            {
                component.Value.EnterScene();
            }

            onEnterScene();
        }

        /// <summary>
        /// 初始化完成后进入场景时执行
        /// </summary>
        protected virtual void onEnterScene() { }

        /// <summary>
        /// 添加组件
        /// </summary>
        /// <param name="component"></param>
        protected T addComponent<T>(T component) where T : AComponent
        {
            if (!_components.TryAdd(component.GetType(), component))
            {
                Debug.Log($"{GetType()} 添加组件 {component.GetType()} Failed!");
            }

            return component;
        }

        /// <summary>
        /// 获取组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>会返回空</returns>
        public T GetComponent<T>() where T : AComponent
        {
            if (!_components.TryGetValue(typeof(T), out var component))
            {
                Debug.Log($"{this.GetType()} 获取组件 {typeof(T)} 失败!");
            }

            return (T)component;
        }

        /// <summary>
        /// 尝试获取组件
        /// </summary>
        /// <param name="comp"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool TryGetComponent<T>(out T comp) where T : AComponent
        {
            comp = null;
            if (_components.TryGetValue(typeof(T), out var component))
            {
                comp = (T)component;
                return true;
            }

            return false;
        }

        protected virtual void onTick(float dt) { }

        public void Tick(float dt)
        {
            _actorInput.Tick(dt);

            foreach (var component in _components)
            {
                component.Value.Tick(dt);
            }
            
            onTick(dt);
        }

        public abstract void RecycleLogicObject();

        public void OnRecycle()
        {
            OnChildRecycle();
            foreach (var component in _components)
            {
                component.Value.Clear();
            }
        }

        protected virtual void OnChildRecycle() { }

        #region 对外接口

        public int GetAttr(EAttrType attrType)
        {
            return Self.GetAttr(attrType);
        }

        public void SetAttr(EAttrType attrType, int value, bool temp)
        {
            Self.SetAttr(attrType, value, temp);
        }

        #endregion
    }

    public static class ActorLogicEx
    {
        public static int GetBuffLayer(this ActorLogic logic, int buffId)
        {
            if (logic.TryGetComponent<ActorLogic.BuffComp>(out var buffComp))
            {
                return buffComp.GetBuffLayer(buffId);
            }

            return -1;
        }

        public static int GetSkillLevel(this ActorLogic logic, int skillId)
        {
            if (logic.TryGetComponent<ActorLogic.SkillComp>(out var skillComp))
            {
                if (skillComp.Skills.TryGetValue(skillId, out var skill))
                {
                    return skill.Level;
                }
            }

            return -1;
        }

        public static void SetSkillLevel(this ActorLogic logic, int skillId, int value)
        {
            if (logic.TryGetComponent<ActorLogic.SkillComp>(out var skillComp))
            {
                if (skillComp.Skills.TryGetValue(skillId, out var skill))
                {
                    skill.Level = value;
                    return;
                }
            }

            Debug.LogError($"[SetSkillLevel] 未找到技能 {skillId}");
        }

        public static void ChangeSkillLevel(this ActorLogic logic, int skillId, int value)
        {
            if (logic.TryGetComponent<ActorLogic.SkillComp>(out var skillComp))
            {
                if (skillComp.Skills.TryGetValue(skillId, out var skill))
                {
                    skill.Level += value;
                    return;
                }
            }

            Debug.LogError($"[SetSkillLevel] 未找到技能 {skillId}");
        }
    }
}