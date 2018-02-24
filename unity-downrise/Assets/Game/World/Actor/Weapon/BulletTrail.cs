using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrail : MonoBehaviour {

	private const float GRAY = 0.6f;
	private const float VISIBILITY = 0.4f;

	private float visibility;
	private LineRenderer line;

	void Start() {

	}

	void Awake () {
		visibility = VISIBILITY;
		line = gameObject.AddComponent<LineRenderer> ();
		line.material.color = new Color (GRAY, GRAY, GRAY, 0);
		line.startWidth = 0.02f;
		line.endWidth = 0.02f;
	}


	void Update () {
		visibility -= Time.deltaTime / 2f;
		if (visibility < 0) {
			Destroy (gameObject);
		}
		line.material.color = new Color (GRAY, GRAY, GRAY, visibility);
	}

	public void Start(Vector2 start) {
		line.SetPosition(0, start);
	}

	public void End(Vector2 end) {
		line.SetPosition(1, end);
	}

	public void Material(Material mat) {
		line.material = mat;
	}

	public void Particle(ParticleSystem particle) {
		ParticleSystem p = GameObject.Instantiate (particle);
		p.transform.parent = transform;
		p.transform.position = line.GetPosition (1);
		p.Play ();
	}
}
