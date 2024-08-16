using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

namespace Battle.Tools
{
    public static class AbilityHelper
    {
        public static void AwardAbility(int actorId, Ability ability, bool isRunNow = false)
        {
            //actor._abilityController.AwardActorAbility(ability, isRunNow);
        }
        
        public static void AwardAbility(this Actor actor, Ability ability, bool isRunNow = false)
        {
            actor.GetAbilityController().AwardActorAbility(ability, isRunNow);
        }
    }
}