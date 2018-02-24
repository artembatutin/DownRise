using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobCombat : ActorBehaviour<Mob> {

	private float speed = 2f;//2 seconds.
	private int damage = 4;//4 hp

	private float timer;
	private Player victim;

	// Use this for initialization
	void Start () {
		timer = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (victim == null)
			return;
		if (Time.time - timer > speed) {
			new Damage (Actor(), victim, damage).Apply();
			timer = Time.time;
		}
	}

	public void Start(Player victim) {
		this.victim = victim;
	}

	public void Stop(Player escape) {
		if (escape.Equals (victim))
			victim = null;
	}
}
