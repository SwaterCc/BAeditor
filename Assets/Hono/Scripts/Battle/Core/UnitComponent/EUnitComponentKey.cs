using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Hono.Scripts.Battle.Core
{
    [Flags]
    public enum EUnitComponentKey
    {
        None = 0,
        CombatComp = 1 << 0,
        HateComp = 1 << 1,
        MoveComp = 1 << 2,
        HpComp = 1 << 3,
        ElementComp = 1 << 4,
        BuffComp = 1 << 5,   
		VehicleComp = 1 << 6,
	}

    public static class EUnitComponentKeyHelper
    {
        #region 初始化数据

        public static readonly Dictionary<Type, EUnitComponentKey> TypeToKeyMap = new();
        public static readonly Dictionary<EUnitComponentKey, Func<UnitComponent>> ComponentFactories = new();
        public static readonly List<EUnitComponentKey> CachedEnumList;

        static EUnitComponentKeyHelper()
        {
            // 初始化枚举列表（排除None）
            CachedEnumList = Enum.GetValues(typeof(EUnitComponentKey))
                .Cast<EUnitComponentKey>()
                .Where(e => e != EUnitComponentKey.None)
                .ToList();

            // 初始化映射关系
            InitializeMappings();
        }

        #endregion

        #region 公共接口

        public static List<EUnitComponentKey> GetList() => CachedEnumList;

        public static UnitComponent GetComponent(this EUnitComponentKey key)
        {
            if (ComponentFactories.TryGetValue(key, out var factory))
            {
                return factory();
            }

            throw new ArgumentOutOfRangeException(nameof(key), key, "No component factory registered");
        }

        public static EUnitComponentKey GetKey<T>() where T : UnitComponent =>
            TypeToKeyMap.TryGetValue(typeof(T), out var key) ? key : EUnitComponentKey.None;

        public static EUnitComponentKey GetKey(this UnitComponent component) =>
            component != null ? GetKey(component.GetType()) : EUnitComponentKey.None;

        #endregion

        #region 核心初始化逻辑

        private static void InitializeMappings()
        {
            // 获取所有UnitComponent的子类
            var componentTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsSubclassOf(typeof(UnitComponent)) &&
                            !t.IsAbstract &&
                            t != typeof(UnitComponent));

            foreach (var type in componentTypes)
            {
                // 建立类型-枚举映射
                if (Enum.TryParse(type.Name, out EUnitComponentKey key))
                {
                    TypeToKeyMap[type] = key;
                    // 建立对象池工厂
                    AddComponentFactory(key, type);
                }
                else
                {
                    Debug.LogError($"Missing EUnitComponentKey for type: {type.Name}");
                }
            }
        }

        private static void AddComponentFactory(EUnitComponentKey key, Type componentType)
        {
            try
            {
                // 通过反射获取泛型池类型
                Type poolType = typeof(GPool<>).MakeGenericType(componentType);

                // 获取Pool属性
                var poolProperty = poolType.GetProperty("Pool", BindingFlags.Public | BindingFlags.Static);

                // 创建工厂委托
                var rentMethod = poolType.GetMethod("Rent", Type.EmptyTypes);
                var factory = (Func<UnitComponent>)Delegate.CreateDelegate(
                    typeof(Func<UnitComponent>),
                    poolProperty.GetValue(null),
                    rentMethod
                );

                ComponentFactories[key] = factory;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Create factory failed for {componentType.Name}: {ex}");
            }
        }

        #endregion

        #region 辅助接口

        private static EUnitComponentKey GetKey(Type componentType) =>
            TypeToKeyMap.TryGetValue(componentType, out var key) ? key : EUnitComponentKey.None;

        #endregion
    }
}