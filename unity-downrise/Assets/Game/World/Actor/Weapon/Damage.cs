using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Damage {

	private Actor attacker;
	private Actor defender;
	private Tile node;
	private float damage;

	public Damage(Actor attacker, GameObject game, Vector2 point, float damage) {
		this.attacker = attacker;
		this.damage = damage;
		if(game.layer == Game.ACTOR_LAYER)
			defender = game.GetComponent<Actor>();
		//else if(game.layer == Game.NODE_LAYER)
		//	node = game.GetComponent<Tile>();
		//Tilemap tm = game.GetComponent<Tilemap>();
		//Debug.Log(new Vector3Int((int)point.x * 2, (int)(point.y * 2), 0));
		//Tile t = tm.GetTile<Tile> (new Vector3Int((int)(point.x * 2), (int)(point.y * 2), 0));
	}

	public Damage(Actor attacker, Actor defender, float damage) {
		this.attacker = attacker;
		this.defender = defender;
		this.damage = damage;
	}

	public Damage(Actor attacker, Tile node, float damage) {
		this.attacker = attacker;
		this.node = node;
		this.damage = damage;
	}

	public Damage(Actor attacker, Actor defender, Tile node, float damage) {
		this.attacker = attacker;
		this.defender = defender;
		this.node = node;
		this.damage = damage;
	}

	public void Apply() {
		if (defender != null)
			defender.Hit (this);
		//if (node != null)
			//node.Hit (this);
	}

	public Actor Attacker() {
		return attacker;
	}

	public bool isPlayer() {
		return attacker != null && attacker.tag.Equals (Game.PLAYER_TAG);
	}

	public Player Player() {
		return attacker.GetComponent<Player>();
	}

	public Actor Defender() {
		return defender;
	}

    public bool onActor() {
        return defender != null;
    }

	public float Value() {
		return damage;
	}
}
