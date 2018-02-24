using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class WeaponTrigger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	private bool triggered = false;

	public void OnPointerUp(PointerEventData e) {
		triggered = false;
	}

	public void OnPointerDown(PointerEventData e) {
		triggered = true;
	}

	public bool isTriggered() {
		return triggered;
	}
}
