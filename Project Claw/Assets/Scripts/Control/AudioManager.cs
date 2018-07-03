using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour {
	public static AudioManager instance;
	public AudioSource audioSource;
	public float masterVolume = 0.5f;
	public float musicVolume = 0.5f;
	public float fxVolume = 0.5f;

	void Awake()
	{
		if ( instance == null )
		{
			instance = this;
		}
		else
		{
			Destroy ( this.gameObject );
		}
		audioSource = gameObject.GetComponent<AudioSource>();
		DontDestroyOnLoad(this.gameObject);
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
		audioSource.Play();
		audioSource.volume = masterVolume * musicVolume;


		PlayerPrefs.SetFloat( "Master volume", masterVolume);
		PlayerPrefs.SetFloat( "Music volume", musicVolume);
		PlayerPrefs.SetFloat( "Sound FX", fxVolume);
	}
	void Update()
	{
//		audioSource.volume = masterVolume * musicVolume;
		if ( Input.GetKeyDown( KeyCode.Space ) )
		{
			Debug.Log("Howdy");
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
		audioSource.volume = masterVolume * musicVolume;
	}
}