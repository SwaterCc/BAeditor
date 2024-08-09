using UnityEngine;

namespace Battle
{
    public class ParamCollection
    {
        private readonly Param[] _params;

        public int Length => _params.Length;
        private readonly int _capacity;

        public ParamCollection(int capacity)
        {
            _capacity = capacity;
            _params = new Param[_capacity];
        }

        public ParamCollection(Param[] paramArr,int capacity = -1)
        {
            _params = paramArr;
            if (capacity <= 0 && capacity < paramArr.Length)
            {
                _capacity = paramArr.Length;  
            }
            else
            {
                _capacity = capacity;
            }
        }

        public bool AddParam(Param param)
        {
            if (Length == _capacity)
            {
                Debug.LogError("AddParam Failed ! Length > capacity");
                return false;
            }
            _params[Length] = param;
            return true;
        }
        
        public bool TryGetParam(int paramIdx, out Param param)
        {
            param = null;
            if (paramIdx >= 0 && paramIdx < Length && _params[paramIdx]!= null)
            {
                param = _params[paramIdx];
                return true;
            }

            return false;
        }

        public Param GetParamAt(int idx)
        {
            if (idx < Length)
            {
                return _params[idx];
            }

            Debug.LogError("Idx > Length !");
            return null;
        }
    }
}