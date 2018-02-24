using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a playable <see cref="Actor"/>.
/// </summary>
public class Player : Actor {
	
	public const int UI_STATS_INDEX = 0;
	public const int UI_WEAPON_INDEX = 1;

	private Animator animator;
	private PlayerStats stats;
	private WeaponContainer weapons;

	private GameObject UIObject;
	private GameObject leftHand;
	private GameObject rightHand;

	private readonly WaitForSeconds deathDelay = new WaitForSeconds(2);

	public override void Initialize() {
		tag = Game.PLAYER_TAG;
		//UI components.
		UIObject = GameObject.Find ("GameUI");
		stats = UIObject.transform.GetChild (UI_STATS_INDEX).gameObject.AddComponent<PlayerStats> ();
		stats.Assign(this);
		weapons = UIObject.transform.GetChild(UI_WEAPON_INDEX).gameObject.AddComponent<WeaponContainer>();
		weapons.Assign(this);
		//Player parts
		leftHand = transform.Find("Body").Find("Sleeve_Left").Find ("Hand_Left").gameObject;
		rightHand = transform.Find("Body").Find("Sleeve_Right").Find ("Hand_Right").gameObject;
		//Managers
		gameObject.AddComponent<PlayerMovement>().Assign(this);
		animator = GetComponentInChildren<Animator>();
	}

	public override void Hit(Damage damage) {
		stats.DecreaseHealth(damage.Value());
	}

	public override IEnumerator Die() {
		yield return deathDelay;
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.R)) {
			stats.DecreaseHealth (40);
		}
		//camera follow
		Game.DR.w.camera.transform.position = new Vector3(transform.position.x, transform.position.y, -1);
	}

	public PlayerStats Stats() {
		return stats;
	}

	public WeaponContainer Weapons() {
		return weapons;
	}

	public override Animator Animator() {
		return animator;
	}

	public GameObject LeftHand() {
		return leftHand;
	}

	public GameObject RightHand() {
		return rightHand;
	}
		
}