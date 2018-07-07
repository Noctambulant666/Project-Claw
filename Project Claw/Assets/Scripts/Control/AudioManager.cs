using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour {
	public static AudioManager instance;
	public float masterVolume = 0.5f;
	public float musicVolume = 0.5f;
	public float fxVolume = 0.5f;

	public Sound[] sounds;

	void Awake()
	{
		if ( instance == null )
		{
			instance = this;
		}
		else
		{
			Destroy ( this.gameObject );
			return;
		}
		DontDestroyOnLoad(this.gameObject);

		foreach( Sound s in sounds )
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.volume = s.volume;
			s.source.loop = s.loop;
		}
	}
	void OnEnable()
	{
		if ( EventManager.Exists )
		{
			EventManager.StartListening( "Change volume", ChangeVolume );
		}
	}
	void OnDisable()
	{
		if ( EventManager.Exists )
		{
			EventManager.StopListening( "Change volume", ChangeVolume );
		}
	}
	void Start()
	{
		PlayerPrefs.SetFloat( "Master volume", masterVolume);
		PlayerPrefs.SetFloat( "Music volume", musicVolume);
		PlayerPrefs.SetFloat( "Sound FX", fxVolume);
	}
	void Update()
	{
		if ( Input.GetKeyDown( KeyCode.Space ) )
		{
			PlayerPrefs.SetFloat( "Master volume", masterVolume);
			PlayerPrefs.SetFloat( "Music volume", musicVolume);
			PlayerPrefs.SetFloat( "Sound FX", fxVolume);
			EventManager.TriggerEvent("Change volume");
		}
	}
	void ChangeVolume()
	{
		if ( PlayerPrefs.HasKey( "Master volume" ) )
		{
			masterVolume = PlayerPrefs.GetFloat( "Master volume" );
		}
		if ( PlayerPrefs.HasKey( "Music volume" ) )
		{
			musicVolume = PlayerPrefs.GetFloat( "Music volume" );
		}
		if ( PlayerPrefs.HasKey( "Sound FX" ) )
		{
			fxVolume = PlayerPrefs.GetFloat( "Sound FX" );
		}

		foreach( Sound s in sounds )
		{
			if ( s.type == SoundType.Music )
				s.source.volume = musicVolume;
			else if ( s.type == SoundType.SoundFX )
				s.source.volume = fxVolume;
			s.source.volume *= masterVolume;
		}
	}
	public void Play( string name )
	{
		Sound sound = Array.Find( sounds, ( Sound s ) => s.name == name );
		if ( sound == null || sound.source == null )
			return;
		if ( sound.source.isPlaying )
		{
			sound.source.Stop();
		}

		if ( sound.type == SoundType.Music )
			sound.source.volume = musicVolume;
		else if ( sound.type == SoundType.SoundFX )
			sound.source.volume = fxVolume;
			
		sound.source.volume *= masterVolume;
		sound.source.Play();
	}
	public void Play( string name, bool keepPlaying )
	{
		// Like play, with an option keepPlaying boolean. If true; see if the sound is playing, and if so it let it
		//    keep playing. If false, it restarts the clip

		Sound sound = Array.Find( sounds, ( Sound s ) => s.name == name );
		if ( sound == null || sound.source == null )
			return;
		if ( sound.source.isPlaying && keepPlaying == true )
		{
			return;
		}
		else if ( sound.source.isPlaying == false && keepPlaying == false )
			sound.source.Stop();

		if ( sound.type == SoundType.Music )
			sound.source.volume = musicVolume;
		else if ( sound.type == SoundType.SoundFX )
			sound.source.volume = fxVolume;

		sound.source.volume *= masterVolume;
		sound.source.Play();
	}
	public void Stop( string name )
	{
		Sound sound = Array.Find( sounds, ( Sound s ) => s.name == name );
		if ( sound == null )
			return;
		sound.source.Stop();
	}
	public void Pause( string name )
	{
		Sound sound = Array.Find( sounds, ( Sound s ) => s.name == name );
		sound.source.Pause();
	}
	public void UnPause( string name )
	{
		Sound sound = Array.Find( sounds, ( Sound s ) => s.name == name );
		sound.source.UnPause();
	}
}
[System.Serializable]
public class Sound
{
	public string name;
	public AudioClip clip;
	[Range(0f,1f)]
	public float volume;
	public bool loop;
	[HideInInspector]
	public AudioSource source;
	public SoundType type;
}
public enum SoundType
{
	Music,
	SoundFX
}