using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponContainer : ActorBehaviour<Player> {

	private const int SKIP_OBJECTS = 2;
	private const int ACTIVE_INDEX = 1;
	private const int WEAPONS_COUNT = 4;

	private Image currentArea;
	private float scrollSwitcher;

	private int selected = 0;
	private int activeCount;
	private Image activeImage;
	private Text activeText;
	private bool locked = false;

	private Weapon[] weapons = new Weapon[WEAPONS_COUNT];
	private Text[] weaponsText = new Text[WEAPONS_COUNT];
	private Image[] weaponsImage = new Image[WEAPONS_COUNT];

	private WeaponTrigger trigger;

    void Start() {
		currentArea = gameObject.transform.GetChild(0).GetComponent<Image>();
		for (int i = SKIP_OBJECTS; i < WEAPONS_COUNT + SKIP_OBJECTS; i++) {
			weaponsText[i - SKIP_OBJECTS] = gameObject.transform.GetChild(i).GetComponentInChildren<Text>();
			weaponsImage[i - SKIP_OBJECTS] = gameObject.transform.GetChild(i).GetComponentInChildren<Image>();
			weaponsImage[i - SKIP_OBJECTS].enabled = false;
			weaponsText[i - SKIP_OBJECTS].text = "";

			WeaponSlot click = weaponsText [i - SKIP_OBJECTS].gameObject.AddComponent<WeaponSlot> ();
			click.Setup (this, i - SKIP_OBJECTS);
		}
		activeImage = transform.GetChild(ACTIVE_INDEX).GetComponentInChildren<Image>();
		activeText = transform.GetChild(ACTIVE_INDEX).GetComponentInChildren<Text>();
		activeImage.enabled = false;
		activeText.text = "";
		trigger = transform.GetChild(ACTIVE_INDEX).gameObject.AddComponent<WeaponTrigger>();
    }

	void Update () {
		if (locked || activeCount == 0)
			return;
		scrollSwitcher += Input.GetAxis("Mouse ScrollWheel");
		if(scrollSwitcher > 1) {
			if(selected + 1 == activeCount) {
				selected = 0;
			} else  {
				selected++;
            }
			Equip();
		} else if(scrollSwitcher < -1) {
			if(selected == 0) {
				selected = activeCount - 1;
			} else {
				selected--;
            }
			Equip();
        }
    }

	public bool Add(Player player, Weapon weapon) {
		if (locked)
			return false;
		if (activeCount >= WEAPONS_COUNT) {
			//throw out old and pick new.
			return false;
		}
		weapon.transform.parent = player.RightHand().transform;
		weapon.transform.localPosition = new Vector2 (0, 0);
		weapon.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90f));
		weapon.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

		weapons[activeCount] = weapon;
		weaponsImage[activeCount].enabled = true;
		weaponsImage[activeCount].sprite = weapon.icon;
		weaponsText [activeCount].text = weapon.Text();
		Equip();
		activeCount++;
		return true;
	}

	public Weapon Selected() {
		return weapons[selected];
	}

	public void Equip(int index) {
		if (weapons [index] != null) {
			selected = index;
			Equip ();
		}
	}

	public void Equip() {
		scrollSwitcher = 0;
		for (int i = 0; i < activeCount; i++) {
			weapons[i].gameObject.SetActive (i == selected);
		}
		if (weapons [selected] != null) {
			activeImage.enabled = true;
			activeImage.sprite = weaponsImage[selected].sprite;
			activeText.text = weaponsText[selected].text;
			currentArea.sprite = Game.DR.w.weaponSprites[selected];
		} else {
			activeImage.enabled = false;
		}
	}

	public void Update(int id) {
		if (id < 0 || id >= weapons.Length)
			return;
		Update(weapons[id], id);
	}

	public void UpdateSelected() {
		Update(weapons [selected], selected);
	}

	public void Update(Weapon wep, int index) {
		if (wep == null)
			return;
		weaponsText[selected].text = wep.Text();
		if(index == selected)
			activeText.text = wep.Text();
	}

	public void Lock() {
		locked = true;
	}

	public void Unlock() {
		locked = false;
	}

	public bool isLocked() {
		return locked;
	}

	public bool ShootAttempt() {
		if (Game.DR.isMobile) {
			return trigger.isTriggered();
		} else {
			return Input.GetKey (KeyCode.Mouse0);
		}
	}

}
