using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {

	public Sprite icon;
	public float damage = 1f;
	public float rate = 0.25f;

	private new AudioSource audio;
	public AudioClip startClip;
	public AudioClip endClip;

	private bool dropped = true;
	private BoxCollider2D groundCollider;

	void Awake() {
		audio = gameObject.AddComponent<AudioSource>();
		if (dropped) {
			SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
			groundCollider = gameObject.AddComponent<BoxCollider2D>();
			groundCollider.isTrigger = true;
			groundCollider.size = sr.size;
		}
	}

	public abstract bool canAttack(Actor a, WeaponContainer container);

	public abstract void Attack(Actor a, ParticleSystem blood);

	public abstract IEnumerator Effect();

	public abstract string Text();

	public void soundStart() {
		if(startClip != null)
			audio.PlayOneShot(startClip);
	}

	public void soundEnd() {
		if (endClip != null)
			audio.PlayOneShot(endClip);
	}

	public AudioSource Audio() {
		return audio;
	}

	void OnTriggerEnter2D(Collider2D o) {
		if (!dropped)
			return;
		if (o.gameObject.tag.Equals(Game.PLAYER_TAG)) {
			Player p = o.gameObject.GetComponent<Player>();
			if (o.gameObject.GetComponent<Player>().Weapons().Add(p, this)) {
				dropped = false;
				Destroy(groundCollider);
				groundCollider = null;
			}
		}
	}
}
