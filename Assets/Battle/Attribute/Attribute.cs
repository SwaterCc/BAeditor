using System.Collections.Generic;
using Battle.Auto;
using Unity.VisualScripting;
using UnityEngine;

namespace Battle
{
    /// <summary>
    /// 属性基类
    /// </summary>
    public abstract class Attribute
    {
        public bool IsComposite => false;

        public abstract ICValueBox GetBox();
    }

    /// <summary>
    /// Set指令缓存Handle接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAttrCommandHandle<T>
    {
        public void Undo(AttrCommand<T> command);
    }

    /// <summary>
    /// 属性改变记录，目前仅记录值类型修改，引用类型的修改需要自己单独继承实现
    /// </summary>
    public class AttrCommand<T>
    {
        /// <summary>
        /// 来源描述，可不写
        /// </summary>
        public string Desc;

        /// <summary>
        /// 修改的值
        /// </summary>
        public T Value { get; }

        public EAttrElementType ElementType { get; }

        private IAttrCommandHandle<T> _commandHandle;

        public AttrCommand(T value, string desc = "") : this(null, value, desc) { }

        public AttrCommand(IAttrCommandHandle<T> handle, T value, string desc = "")
        {
            _commandHandle = handle;
            Value = value;
            Desc = desc;
        }
        
        /// <summary>
        /// 复合属性调用
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="elementType"></param>
        /// <param name="value"></param>
        /// <param name="desc"></param>
        public AttrCommand(IAttrCommandHandle<T> handle,EAttrElementType elementType ,T value, string desc = "")
        {
            _commandHandle = handle;
            Value = value;
            Desc = desc;
            ElementType = elementType;
        }

        /// <summary>
        /// 如果未绑定handle，这个方法可以绑定handle，已经绑定的不允许修改
        /// </summary>
        /// <param name="handle"></param>
        public void BindHandle(IAttrCommandHandle<T> handle)
        {
            _commandHandle ??= handle;
        }

        /// <summary>
        /// 撤销这次修改
        /// </summary>
        public void Undo()
        {
            if (_commandHandle == null)
            {
                Debug.LogWarning("commandHandle not Init Undo failed!");
            }
            else
            {
                _commandHandle.Undo(this);
            }
        }
    }
}