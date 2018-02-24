using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Effects;

public class Mob : Actor {

	public float movementSpeed = 0.6f;
	public float health = 100f;

	private MobCombat combat;
    private MobAwarenes awarenes;
	private Animator animator;

	private readonly WaitForSeconds deathDelay = new WaitForSeconds(1f);

	public override void Initialize () {
        animator = gameObject.GetComponentInChildren<Animator> ();
		awarenes = gameObject.GetComponentInChildren<MobAwarenes>();
		gameObject.AddComponent<MobMovement>().Speed(0.3f).Assign(this);
		combat = gameObject.AddComponent<MobCombat>();
		combat.Assign(this);
    }

	public override void Hit(Damage damage) {
		//attacking player back.
		if (damage.isPlayer ()) {
			Player attacker = damage.Player();
			if (awarenes.TargetInRange () == null) {
				awarenes.Target (attacker);
			} else if (damage.isPlayer()) {
				if (!awarenes.TargetInRange ().Equals (attacker)) {

				}
			}
		}
		//decreasing health.
		health -= damage.Value();
		if (health <= 0) {
			StartCoroutine(Die());
		}
	}

	public override IEnumerator Die() {
		GameObject.Destroy (gameObject);
		GameObject death = GameObject.Instantiate(Game.DR.w.mobDeath);
		death.transform.position = transform.position;
		yield return deathDelay;
		GameObject.Destroy (death);
	}

	public override Animator Animator () {
		return animator;
	}

	public MobCombat Combat() {
		return combat;
	}

	public MobAwarenes Awareness() {
		return awarenes;
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == Game.PLAYER_TAG) {
			combat.Start(coll.gameObject.GetComponent<Player>());
		}
	}

	void OnCollisionExit2D(Collision2D coll) {
		if (coll.gameObject.tag == Game.PLAYER_TAG) {
			combat.Stop(coll.gameObject.GetComponent<Player>());
		}
	}
}
