using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the time clock and the sun intensity.
/// </summary>
public class TimeSystem : MonoBehaviour {

	/// <summary>
	/// The name of the object containing the time text.
	/// </summary>
	private readonly string TIME_OBJECT = "Time";
	/// <summary>
	/// Increases a minute to the time after the amount of seconds has elapsed.
	/// </summary>
	private readonly float MINUTE_INCREASE = 10;
	/// <summary>
	/// The sun intensity at mid night.
	/// </summary>
	private readonly float MIDNIGHT_INTENSITY = 0;
	/// <summary>
	/// The sun intensity at lunch time.
	/// </summary>
	private readonly float LUNCH_INTENSITY = 0.8F;

	private Text text;

	private float lightFactor;

	private float currentSpan;
	private bool pm;
	private int hours;
	private int minutes;

	public TimeSystem() {
		lightFactor = (LUNCH_INTENSITY - MIDNIGHT_INTENSITY) / (60 * 12);
	}
		
	void Start() {
		text = GameObject.Find(TIME_OBJECT).GetComponent<Text>();
	}

	void Update () {
		currentSpan += Time.deltaTime;
		if (currentSpan > MINUTE_INCREASE) {

			//Clock manipulation.
			minutes++;
			if (minutes == 60) {
				minutes = 0;
				hours++;
				if (hours == 12 && !pm) {
					pm = true;
				} else if (hours == 12 && pm) {
					hours = 0;
					pm = false;
				}
			}
			//sun.intensity += pm ? -lightFactor : lightFactor;
			UpdateText();
			currentSpan -= MINUTE_INCREASE;
		}
	}

	/// <summary>
	/// Updates the clock text.
	/// </summary>
	private void UpdateText() {
		int hoursCount = Count (hours);
		int minutesCount = Count (minutes);
		text.text = (hoursCount == 1 ? "0" : "") + hours + " : " + (minutesCount == 1 ? "0" : "") + minutes + (pm ? " PM" : " AM");
	}


	public static int Count(int Value) { 
		return ((Value == 0) ? 1 : ((int)Mathf.Floor(Mathf.Log10(Mathf.Abs(Value))) + 1)); 
	}
}
