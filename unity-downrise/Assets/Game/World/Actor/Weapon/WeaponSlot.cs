using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class WeaponSlot : MonoBehaviour, IPointerDownHandler {

	private int index;
	private WeaponContainer container;

	public void Setup (WeaponContainer container, int index) {
		this.container = container;
		this.index = index;
	}
	
	public void OnPointerDown(PointerEventData e) {
		if(!container.isLocked())
			container.Equip (index);
	}
}
