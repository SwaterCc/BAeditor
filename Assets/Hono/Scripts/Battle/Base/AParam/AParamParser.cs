using System;
using UnityEngine;

namespace Hono.Scripts.Battle.Base
{
    public partial class AParamParser
    {
        private readonly IntParser _intParser = new();
        private readonly FloatParser _floatParser = new();
        private readonly BooleanParser _booleanParser = new();
        private readonly Vector3Parser _v3Parser = new();
        private readonly RefParser _refParser = new();

        /// <summary>
        /// 解析获得Int
        /// </summary>
        /// <param name="ability"></param>
        /// <param name="aParams"></param>
        /// <returns></returns>
        public int ParseInt(Ability ability, AParams aParams)
        {
            try
            {
                return _intParser.Parse(ability, aParams);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return 0;
            }
        }

        /// <summary>
        /// 解析获取float
        /// </summary>
        /// <param name="ability"></param>
        /// <param name="aParams"></param>
        /// <returns></returns>
        public float ParseFloat(Ability ability, AParams aParams)
        {
            try
            {
                return _floatParser.Parse(ability, aParams);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return 0;
            }
        }

        /// <summary>
        /// 解析获取bool
        /// </summary>
        /// <param name="ability"></param>
        /// <param name="aParams"></param>
        /// <returns></returns>
        public bool ParseBoolean(Ability ability, AParams aParams)
        {
            try
            {
                return _booleanParser.Parse(ability, aParams);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }
        
        /// <summary>
        /// 解析获取Vector3
        /// </summary>
        /// <param name="ability"></param>
        /// <param name="aParams"></param>
        /// <returns></returns>
        public Vector3 ParseVector3(Ability ability, AParams aParams)
        {
            try
            {
                return _v3Parser.Parse(ability, aParams);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return Vector3.zero;
            }
        }

        /// <summary>
        /// 解析引用对象
        /// </summary>
        /// <param name="ability"></param>
        /// <param name="aParams"></param>
        /// <returns></returns>
        public object ParseRef(Ability ability, AParams aParams)
        {
            try
            {
                return _refParser.Parse(ability, aParams);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        /// <summary>
        /// 解析对象
        /// </summary>
        /// <param name="ability"></param>
        /// <param name="aParams"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T ParseRef<T>(Ability ability, AParams aParams) where T : class
        {
            try
            {
                return (T)_refParser.Parse(ability, aParams);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }
    }
}