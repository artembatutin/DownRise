using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The main Game instance controller.
/// </summary>
public class Game {

	public const int TILE_LAYER = 9;
	public const int ACTOR_LAYER = 10;
	public const int NODE_LAYER = 11;

	public const string PLAYER_TAG = "P";

	/// <summary>
	/// DownRise <code>Game</code> instance.
	/// </summary>
	private static Game instance;
	/// <summary>
	/// Proprety gathering and creating the Game instance.
	/// </summary>
	public static Game DR {
		get {
			if (instance == null)
				instance = new Game ();
			return instance;
		}
	}

	/// <summary>
	/// The World instance.
	/// </summary>
	public World w;
	public bool isMobile = true;

	//Audio clips
	public AudioCache audio;

	/// <summary>
	/// Initializes a new instance of the <see cref="Game"/> class.
	/// </summary>
	private Game() {
		w = GameObject.Find("World").GetComponent<World>();
		audio = new AudioCache().Load();
		#if UNITY_STANDALONE
			isMobile = false;
		#endif
	}
}
