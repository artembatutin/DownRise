using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {

	private Image imgJoystick;
	private Image imgBackground;
	public static bool inUse;
	public static Vector3 input;

	void Start() {
		if (!Game.DR.isMobile) {
			GameObject.Destroy (gameObject);
			return;
		}
		imgBackground = GetComponent<Image> ();
		imgJoystick = transform.GetChild(0).GetComponent<Image> ();
	}

	public void OnPointerUp(PointerEventData e) {
		inUse = false;
		input = Vector3.zero;
		imgJoystick.rectTransform.anchoredPosition = Vector3.zero;
	}

	public void OnPointerDown(PointerEventData e) {
		inUse = true;
		OnDrag(e);
	}

	public void OnDrag(PointerEventData e) {
		Vector2 pos;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle(imgBackground.rectTransform,
			e.position,
			e.pressEventCamera,
			out pos)) {

			pos.x = (pos.x / imgBackground.rectTransform.sizeDelta.x);
			pos.y = (pos.y / imgBackground.rectTransform.sizeDelta.y);

			input = new Vector3(pos.x * 2, 0, pos.y * 2);
			input = (input.magnitude > 1.0f) ? input.normalized : input;

			imgJoystick.rectTransform.anchoredPosition = new Vector3(
				input.x * (imgBackground.rectTransform.sizeDelta.x * 0.4f),
				input.z * (imgBackground.rectTransform.sizeDelta.y * 0.4f));
		}
	}   

}