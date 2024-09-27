using System;
using Cysharp.Threading.Tasks;
using Hono.Scripts.Battle.Tools;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Hono.Scripts.Battle
{
    public class AlwaysShow : ActorShow
    {
        public AlwaysShow(Actor actor) : base(actor) { }
    }
}