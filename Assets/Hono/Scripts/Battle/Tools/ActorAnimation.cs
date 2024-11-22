#region

using Hono.Scripts.Battle;
using System;
using UnityEngine;

#endregion

public class ActorAnimation : MonoBehaviour {
	public ActorModel model;
	private EActorStateType Before;
	public Animator animator;

	private Actor _actor;

	void Update() {
		_actor ??= ActorManager.Instance.GetActor(model.ActorUid);

		if (_actor.Logic.CurState() != Before) {
			if (animator != null) {
				animator.Play(Enum.GetName(typeof(EActorStateType), _actor.Logic.CurState()));
			}

			Before = _actor.Logic.CurState();
		}
	}
}