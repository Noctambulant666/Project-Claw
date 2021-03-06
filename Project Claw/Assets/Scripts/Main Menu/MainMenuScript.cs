﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {
	public GameObject currentPanel = null;

	void Start()
	{
		AudioManager.instance.Play( "Theme", false );
	}
	public void Exit()
	{
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Exit();
		#endif
	}
	public void StartGame()
	{
//		SceneManager.LoadScene( "Level 1");
		StartCoroutine( Starting() );
	}
	public void SwitchPanel( GameObject nextPanel )
	{
		currentPanel.SetActive(false);
		nextPanel.SetActive(true);
		currentPanel = nextPanel;
	}
	IEnumerator Starting()
	{
		EventManager.TriggerEvent( "Fade out");
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene("Level 1");
	}
}