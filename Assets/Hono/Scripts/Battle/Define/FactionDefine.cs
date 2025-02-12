using System.Collections.Generic;
using Hono.Scripts.Battle.Core;

namespace Hono.Scripts.Battle
{
    public static class Faction
    {
        private class Relationship
        {
            public readonly HashSet<int> Friendly;
            public readonly HashSet<int> Enemy;

            public Relationship(int[] friendly, int[] enemy)
            {
                Friendly = new HashSet<int>();
                Enemy = new HashSet<int>();
                foreach (var id in friendly)
                {
                    Friendly.Add(id);
                }

                foreach (var id in enemy)
                {
                    Enemy.Add(id);
                }
            }
        }

        private static HashSet<int> _factionIds = new HashSet<int>()
        {
            1, 2, 3, 4, 5, 6, 7, 8, 9
        };

        private static readonly Dictionary<int, Relationship> _defines = new()
        {
            { 1, new(new[] { 1 }, new[] { 3, 4, 5, 6, 7, 8, 9 }) },
            { 2, new(new[] { 2 }, new[] { 3, 4, 5, 6, 7, 8, 9 }) },
            { 5, new(new[] { 3, 4, 5, 6, 7, 8, 9 }, new[] { 1, 2 }) },
            { 6, new(new[] { 3, 4, 5, 6, 7, 8, 9 }, new[] { 1, 2 }) },
            { 7, new(new[] { 3, 4, 5, 6, 7, 8, 9 }, new[] { 1, 2 }) },
            { 4, new(new[] { 3, 4, 5, 6, 7, 8, 9 }, new[] { 1, 2 }) },
        };

        public static EFactionRelationship GetRelationship(int factionId, int checkId)
        {
            if (!_defines.TryGetValue(factionId, out var relationship))
            {
                return EFactionRelationship.Neutrality;
            }

            if (relationship.Friendly.Contains(checkId))
            {
                return EFactionRelationship.Friendly;
            }

            if (relationship.Enemy.Contains(checkId))
            {
                return EFactionRelationship.Enemy;
            }

            return EFactionRelationship.Neutrality;
        }

        public static EFactionRelationship GetWithUnitRelationship(this Unit unit, Unit check)
        {
            var factionId = unit.GetAttr(EAttrType.AttrFaction);
            var checkId = check.GetAttr(EAttrType.AttrFaction);
            return GetRelationship(factionId, checkId);
        }

        public static bool IsSpecialRelationship(Unit unit, Unit check, EFactionRelationship factionRelationship)
        {
            var factionId = unit.GetAttr(EAttrType.AttrFaction);
            var checkId = check.GetAttr(EAttrType.AttrFaction);
            return GetRelationship(factionId, checkId) == factionRelationship;
        }
    }
}