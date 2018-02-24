using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An implementation of MonoBehaviour which handles control over an actor.
/// </summary>
public abstract class ActorBehaviour<A> : MonoBehaviour where A : Actor {

	/// <summary>
	/// The actor (player/mob) in control of this behaviour.
	/// </summary>
	private A actor;

	public ActorBehaviour<A> Assign(A actor) {
		this.actor = actor;
		return this;
	}

	/// <summary>
	/// Getting the actor of this behaviour.
	/// </summary>
	public A Actor() {
		return actor;
	}
}
