using System.Collections.Generic;

namespace Battle
{
    /// <summary>
    /// 复合属性，结果为多个值计算得出的数据，例如HP，MP，攻击力，防御力这种
    /// </summary>
    public class CompositeAttribute : Attribute, IAttrCommandHandle<float>
    {
        /// <summary>
        /// 复合属性成员，用来表示，属性基础，属性加值，属性乘值等等
        /// </summary>
        public class AttributeElement
        {
            private Attribute _attr;

            public EAttrElementType Type;

            public EAttrCommandType CommandType;
            
            private bool _dirty;

            private float _curValue;

            private LinkedList<AttrCommand<float>> _commands;

            private bool _openCommandCache;

            public AttributeElement(Attribute attr, EAttrElementType type, LinkedList<AttrCommand<float>> commands)
            {
                _attr = attr;
                Type = type;
                _commands = commands;
                _openCommandCache = commands != null;
            }

            /// <summary>
            /// 指令变动时更新值
            /// </summary>
            private void updateValue()
            {
                if (CommandType == EAttrCommandType.Add)
                {
                    foreach (var command in _commands)
                    {
                        _curValue += command.Value;
                    }
                }
                else
                {
                    _curValue = _commands.Last.Value.Value;
                }
            }

            public void Set(float value)
            {
                if (_openCommandCache)
                {
                    updateValue();
                }
                else
                {
                    _curValue = value;
                }
            }

            public float GetValue()
            {
                return 0;
            }
        }

        /// <summary>
        /// 重写，设置为复合属性
        /// </summary>
        public new bool IsComposite => true;

        private readonly bool _openCommandCache;
        private int _formulaId;
        private float _base;
        private float _final;

        private readonly Dictionary<EAttrElementType, AttributeElement> _elements;
        private readonly Dictionary<EAttrElementType, LinkedList<AttrCommand<float>>> _commandDict;

        public CompositeAttribute(EAttrElementType[] elementTypes, int formulaId, float baseValue ,bool openCommandCache = true)
        {
            _elements = new Dictionary<EAttrElementType, AttributeElement>(4);
            _formulaId = formulaId;
            _openCommandCache = openCommandCache;
            _base = baseValue;
            
            if (_openCommandCache)
            {
                _commandDict = new Dictionary<EAttrElementType, LinkedList<AttrCommand<float>>>();
            }

            foreach (var elementType in elementTypes)
            {
                if (elementType is EAttrElementType.Base or EAttrElementType.Final)
                    continue;
                var commands = _openCommandCache ? new LinkedList<AttrCommand<float>>() : null;
                var element = new AttributeElement(this, elementType, commands);

                _elements.Add(elementType, element);
                _commandDict?.Add(elementType, commands);
            }

            calculateFinal();
        }

        /// <summary>
        /// 根据公式计算最终值
        /// </summary>
        private void calculateFinal()
        {
            _final = _base;
        }

        public new float Get()
        {
            return GetFinal();
        }

        public float GetBase()
        {
            return _base;
        }

        public float GetFinal()
        {
            return _final;
        }

        public AttrCommand<float> SetElementAttr(EAttrElementType type, float value)
        {
            if (!_elements.TryGetValue(type, out var element))
            {
                return null;
            }


            if (_openCommandCache)
            {
                if (!_commandDict.TryGetValue(type, out var commandList)) return null;
                var command = new AttrCommand<float>(this, type, value);
                commandList.AddLast(command);
                element.Set(value);
                return command;
            }
            else
            {
                element.Set(value);
            }

            return null;
        }

        public void Undo(AttrCommand<float> command)
        {
            if (_openCommandCache && _commandDict.TryGetValue(command.ElementType, out var commandList))
            {
                var node = commandList.Find(command);
                if (node != null)
                {
                    commandList.Remove(command);
                }
            }
        }

        public void UndoAndClear(ref AttrCommand<float> command)
        {
            if (_openCommandCache && _commandDict.TryGetValue(command.ElementType, out var commandList))
            {
                var node = commandList.Find(command);
                if (node != null)
                {
                    commandList.Remove(command);
                    command = null;
                }
            }
        }
    }
}