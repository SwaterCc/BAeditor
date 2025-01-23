using System.Collections;
using System.Collections.Generic;

namespace Hono.Scripts.Battle.Base
{
    public partial class AttrCollection
    {
        public class AttrSnapshots : IAPoolObject, IEnumerable<KeyValuePair<EAttrType, int>>
        {
            private readonly Dictionary<EAttrType, int> _snapshot = new(128);
            private readonly List<int> _param = new(5);

            public void Init(AttrCollection collection)
            {
                foreach (var attr in collection._attrs)
                {
                    _snapshot.Add(attr.Key, attr.Value);
                }
            }

            public void AddParam(int param)
            {
                _param.Add(param);
            }

            public void Process(string rule) { }

            public IEnumerator<KeyValuePair<EAttrType, int>> GetEnumerator()
            {
                foreach (var num in _snapshot)
                {
                    yield return num;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public void OnRecycle()
            {
                _snapshot.Clear();
                _param.Clear();
            }
        }
    }
}