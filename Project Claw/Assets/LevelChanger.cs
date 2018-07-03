using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LevelChanger : MonoBehaviour {
	public Animator animator;
	public static LevelChanger instance;

	void Awake()
	{
		if ( instance == null )
		{
			instance = this;
		}
		else
		{
			Destroy(this.gameObject);
		}
		animator = gameObject.GetComponent<Animator>();
		DontDestroyOnLoad(this.gameObject);
	}
	void OnEnable()
	{
		if ( EventManager.Exists)
		{
			EventManager.StartListening( "Fade in", FadeIn );
			EventManager.StartListening( "Fade out", FadeOut);
		}
	}
	void OnDisable()
	{
		if ( EventManager.Exists )
		{
			EventManager.StopListening( "Fade in", FadeIn );
			EventManager.StopListening( "Fade out", FadeOut);
		}
	}
	void FadeIn()
	{
		animator.SetTrigger( "FadeIn");
		StartCoroutine( FadeTimer() );
	}
	void FadeOut()
	{
		animator.SetTrigger( "FadeOut" );
		StartCoroutine( FadeTimer() );
	}
	IEnumerator FadeTimer()
	{
		yield return new WaitForSeconds(1f);
		EventManager.TriggerEvent("Faded");
	}
}
