using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundType {
	SOUND_EFFECT,
	MUSIC
}

[System.Serializable]
public class Sound {

	[SerializeField]
	private string _name;
	public string name { get => _name; private set => _name = value; }

	[SerializeField]
	private AudioClip _audioClip;
	public AudioClip clip { get => _audioClip; private set => _audioClip = value; }

	[SerializeField]
	private SoundType _type;
	public SoundType type { get => _type; private set => _type = value; }

	[SerializeField]
	private bool _loop;
	public bool loop { get => _loop; private set => _loop = value; }

	[Range(0.0f, 1.0f)]
	[SerializeField]
	private float _volume = 0.5f;
	public float volume { get => _volume; private set => _volume = value; }

	[Range(0.1f, 3.0f)]
	[SerializeField]
	private float _pitch = 1.0f;
	public float pitch { get => _pitch; private set => _pitch = value; }

	public AudioSource source { get; set; }
}