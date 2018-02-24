using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controlls the player's movement behaviour.
/// </summary>
public class PlayerMovement : ActorBehaviour<Player> {

	/// <summary>
	/// The delay of the step audio.
	/// </summary>
	private const int STEP_AUDIO_DELAY = 14;
	private const string STEP_CLIPS_NAME = "footstep";
	private const int STEP_CLIPS_LENGTH = 9;

	/// <summary>
	/// The maximal velocity the player can achieve. 
	/// </summary>
	/// //WEIGHT DECREASES TO MIN 1.7
	/// //WAS 2.3F
	private const float MAX_VELOCITY = 1.4f;
	private const float MOVABLE_JOYSTICK_OUT = 0.4f;
	private const float STRAIGHT_CAP = 0.1f;
	private const float CURVED_CAP = 0.35f;
	private const float CURVED_DIVIDER = 1.05f;
	private const float STOPPED_DIVIDER = 1.4f;

	//The idle animation stage.
	private const int IDDLE_ANIM = 0;
	//The walking animation stage.
	private const int WALK_ANIM = 1;

	//Step count synchronizer.
	private int stepSync;
	//Step time delay.
	private float stepDelay;
	//Step walking speed.
	private float stepSpeed = 0.5F;

	//If the player is in movement.
	private bool inMovement = false;

	//The look vector of the player.
	private Vector2 look;
	//The previous rotation for smoothness.
	private Quaternion prevRotation;

	void FixedUpdate() {
		HandleRotate ();
		HandleMovement ();
		CapVelocity ();
	}

	/// <summary>
	/// Handling rotation(to the mouse), also saves the look orientation.
	/// </summary>
	public void HandleRotate() {
		if (Game.DR.isMobile && Joystick.inUse) {
			//rotate to joystick
			Vector3 joystick = Joystick.input;
			float rad = Mathf.Atan2 (joystick.z, joystick.x);
			float angle = rad * Mathf.Rad2Deg;
			Actor().transform.rotation = Quaternion.Lerp (prevRotation, Quaternion.Euler (new Vector3 (0, 0, angle)), 0.5f);
			prevRotation = Actor().transform.rotation;
			//Smoothen the look direction vector.
			float x = joystick.x;
			float y = joystick.z;
			if (x < STRAIGHT_CAP && x > -STRAIGHT_CAP)
				x = 0;
			else if (x < CURVED_CAP && x > -CURVED_CAP)
				x /= 10f;
			if (y < STRAIGHT_CAP && y > -STRAIGHT_CAP)
				y = 0;
			else if (y < CURVED_CAP && y > -CURVED_CAP)
				y /= 10f;
			look = new Vector2 (x, y);
		} else if(!Game.DR.isMobile) {
			//rotate to mouse
			Vector3 mousePos = Input.mousePosition;
			mousePos.z = 10f;
			Vector3 objectPos = Game.DR.w.camera.WorldToScreenPoint (Actor().transform.position);
			mousePos.x = mousePos.x - objectPos.x;
			mousePos.y = mousePos.y - objectPos.y;
			float rad = Mathf.Atan2 (mousePos.y, mousePos.x);
			float angle = rad * Mathf.Rad2Deg;
			Actor().transform.rotation = Quaternion.Lerp (prevRotation, Quaternion.Euler (new Vector3 (0, 0, angle)), 0.5f);
			prevRotation = Actor().transform.rotation;
			//Smoothen the look direction vector.
			float x = Mathf.Cos (rad);
			float y = Mathf.Sin (rad);
			if (x < STRAIGHT_CAP && x > -STRAIGHT_CAP)
				x = 0;
			else if (x < CURVED_CAP && x > -CURVED_CAP)
				x /= 10f;
			if (y < STRAIGHT_CAP && y > -STRAIGHT_CAP)
				y = 0;
			else if (y < CURVED_CAP && y > -CURVED_CAP)
				y /= 10f;
			look = new Vector2 (x, y);
		}
	}

	/// <summary>
	/// Handles the step-system movement.
	/// </summary>
	public void HandleMovement() {
		//movements
		stepDelay += Time.deltaTime;
		if (stepDelay < stepSpeed)
			return;
		inMovement = true;
		float variantSpeed = 10 * (7 - stepSpeed / 0.05F);

		Vector3 joy = Joystick.input;
		bool move = !Game.DR.isMobile ? Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.UpArrow)
			: joy.x < -MOVABLE_JOYSTICK_OUT || joy.x > MOVABLE_JOYSTICK_OUT || joy.z > MOVABLE_JOYSTICK_OUT || joy.z < -MOVABLE_JOYSTICK_OUT;

		if (move) {
			Actor().Body().AddForce (look * variantSpeed);
			Actor().Animator().SetInteger("State", WALK_ANIM);
			if (stepSpeed > 0.15)
				stepSpeed -= 0.025f;
		} else if (Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.DownArrow)) {
			Actor().Body().AddForce (-look * variantSpeed);
			Actor().Animator().SetInteger("State", WALK_ANIM);
		} else {
			inMovement = false;
			stepSpeed = 0.3F;
			stepSync = 0;
			Actor().Animator().SetInteger("State", IDDLE_ANIM);
		}
		if (inMovement && stepSync++ > STEP_AUDIO_DELAY) {
			stepDelay = 0;
			AudioClip step = Game.DR.audio.Get(STEP_CLIPS_NAME + Random.Range(0, STEP_CLIPS_LENGTH));
			Actor().Audio().PlayOneShot (step, Random.Range(0.3f, 0.7f));
			stepSync = 0;
		}
	}

	/// <summary>
	/// Caps velocity movement to not exceed <code>velCap</code>.
	/// </summary>
	public void CapVelocity() {
		//velocity capping
		Vector2 vel = Actor().Body().velocity;
		if(vel.x > MAX_VELOCITY) {
			vel.x = MAX_VELOCITY;
		} else if(vel.x < -MAX_VELOCITY) {
			vel.x = -MAX_VELOCITY;
		} else if(!inMovement) {
			vel.x = 0;
		} else {
			vel.x /= CURVED_DIVIDER;
		}
		if(vel.y > MAX_VELOCITY) {
			vel.y = MAX_VELOCITY;
		} else if(vel.y < -MAX_VELOCITY) {
			vel.y = -MAX_VELOCITY;
		} else if(!inMovement) {
			vel.y = 0;
		} else {
			vel.y /= CURVED_DIVIDER;
		}
		Actor().Body().velocity = vel;
	}
}
