using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Caches audio clip files.
/// </summary>
public class AudioCache {

	/// <summary>
	/// Default audio location.
	/// </summary>
	private readonly string PATH = "Audio/";

	//The clips and linkage to them.
	private AudioClip[] clips;
	private Dictionary<string, AudioClip> links = new Dictionary<string, AudioClip>();

	/// <summary>
	/// Loads all clips into an array.
	/// </summary>
	public AudioCache Load() {
		clips = Resources.LoadAll<AudioClip>(PATH);
		foreach (AudioClip clip in clips) {
			links.Add(clip.name, clip);
		}
		return this;
	}

	/// <summary>
	/// Gathering an audio clip file.
	/// </summary>
	/// <param name="name">Clip file name</param>
	public AudioClip Get(string name) {
		return links[name];
	}
}

