using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon {

	public const float RELOAD_WAIT = 3f;

	[SerializeField]
	private int magazineSize = 7;
	[SerializeField]
	private float range = 50f;
	[SerializeField]
	private float reloadTime = 3.2f;
	[SerializeField]
	private AudioClip reloadClip;
	[SerializeField]
	private ParticleSystem spark;

	private WaitForSeconds shotWait;
	private WaitForSeconds reloadWait;
	private bool reloading = false;

	[HideInInspector]
	private int magazineFill;
	[HideInInspector]
	private Transform bulletStart;
	[HideInInspector]
	private ParticleSystem muzzleFlash;

	public override string Text() {
		return magazineFill + " / " + magazineSize;
	}

	void Start () {
		reloading = false;
		magazineFill = magazineSize;
		bulletStart = transform.GetChild(0).transform;
		muzzleFlash = gameObject.GetComponentInChildren<ParticleSystem> ();
		shotWait = new WaitForSeconds(rate * 0.8f);
		reloadWait = new WaitForSeconds (reloadTime);
	}

	public override bool canAttack(Actor a, WeaponContainer container) {
		if (reloading)
			return false;
		magazineFill--;
		if (magazineFill < 0) {
			reloading = true;
			StartCoroutine(Reload(a, container));
			return false;
		}
		return true;
	}

	public override void Attack(Actor a, ParticleSystem blood) {
		Vector2 start = bulletStart.position;
		Vector2 end = start + a.Forward() * 15;
		RaycastHit2D hit;
		if (hit = Physics2D.Raycast (start, a.Forward (), range)) {
			end = new Vector2 (hit.point.x, hit.point.y);
			Damage d = new Damage (a, hit.collider.gameObject, hit.point, damage);
			if (d.onActor ()) {
				Bullet (start, end, blood);
			} else {
				Bullet (start, end, spark);
			}
			d.Apply ();
		} else {
			Bullet(start, end, null);
		}
	}

	public override IEnumerator Effect() {
		if (muzzleFlash != null) {
			muzzleFlash.Play ();
		}
		soundStart();
		yield return shotWait;
		if(muzzleFlash != null)
			muzzleFlash.Stop();
	}

	private void Bullet(Vector2 start, Vector2 end, ParticleSystem splash) {
		//bullet effect
		GameObject o = new GameObject ();
		o.name = "Bullet";
		o.transform.position = end;

		BulletTrail trail = o.AddComponent<BulletTrail>();
		trail.Material (Game.DR.w.spriteMat);
		trail.Start(start);
		trail.End(end);
		if(splash != null)
			trail.Particle(splash);

	}

	public IEnumerator Reload(Actor a, WeaponContainer container) {
		container.Lock ();
		PlayReload();
		a.Animator ().SetBool ("Reloading", true);
		yield return reloadWait;
		magazineFill = magazineSize;
		container.UpdateSelected();
		reloading = false;
		container.Unlock ();
		a.Animator ().SetBool ("Reloading", false);
	}

	public void PlayReload() {
		if (reloadClip != null && Audio() != null)
			Audio().PlayOneShot (reloadClip, 0.5f);
	}
}
