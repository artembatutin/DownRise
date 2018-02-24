using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class World : MonoBehaviour {

	//STORAGE-REFEREMCE
	//World camera instance.
	public new Camera camera;

	// RENDERING
	//Default font.
	public Font font;
	//Default sprite material.
	public Material spriteMat;

	//Desktop actor material.
	public Material deskActorMat;
	//Desktop node material.
	public Material deskNodeMat;
	//Desktop ground material.
	public Material deskGroundMat;

	//SPRITES
	//Weapon area sprites.
	public Sprite[] weaponSprites;
	//Hitbar sprite.
	public Sprite health;

	//EFFECTS
	public GameObject mobDeath;

    void Awake() {
		Application.targetFrameRate = 60;
		//Adds the time system clock.
		gameObject.AddComponent<TimeSystem> ();
    }
		
    void Update() {
		//Save-quit with key "P".
        if (Input.GetKeyDown(KeyCode.P)) {
            Application.Quit();
            //UnityEditor.EditorApplication.isPlaying = false;
        }
    }

}
