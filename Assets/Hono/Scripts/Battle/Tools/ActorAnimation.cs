#region

using System;
using Hono.Scripts.Battle;
using UnityEngine;

#endregion

public class ActorAnimation : MonoBehaviour
{
    public ActorModel model;
    private EActorLogicStateType Before;
    public Animator animator;

    private Actor _actor;

    void Update()
    {
        _actor ??= ActorManager.Instance.GetActor(model.ActorUid);

        if (_actor.Logic.CurState() != Before)
        {
            if (animator != null)
            {
                animator.Play(Enum.GetName(typeof(EActorLogicStateType), _actor.Logic.CurState()));
            }

            Before = _actor.Logic.CurState();
        }
    }
}