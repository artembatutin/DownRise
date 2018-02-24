using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobAwarenes : ActorBehaviour<Mob> {

	//radius of the field of view.
	public float fovRadius;
	//offset of the field of view.
	public Vector2 fovOffset;

    private Player player;
    private CircleCollider2D fieldOfView;

    void Start() {
        fovRadius = 2;
        fovOffset = new Vector2(0, 1);
        fieldOfView = gameObject.AddComponent<CircleCollider2D>();
        fieldOfView.radius = fovRadius;
        fieldOfView.offset = fovOffset;
        fieldOfView.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == Game.PLAYER_TAG) {
            player = collision.gameObject.GetComponent<Player>();
        }
    }

	public void Target(Player player) {
		this.player = player;
	}

    public Player TargetInRange() {
        return player;
    }
}