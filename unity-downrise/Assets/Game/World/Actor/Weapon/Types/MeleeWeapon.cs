using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon {

	public override bool canAttack(Actor a, WeaponContainer container) {
		return true;
	}

	public override void Attack(Actor a, ParticleSystem blood) {

	}

	public override IEnumerator Effect() {
		return null;
	}

	public override string Text() {
		return "";
	}

}
