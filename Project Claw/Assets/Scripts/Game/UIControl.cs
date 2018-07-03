using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControl : MonoBehaviour {
	public GameObject menu;

	void OnEnable()
	{
		if ( GameObject.Find("Event Manager") != null )
		{
			EventManager.StartListening( "End Level", LevelComplete );
		}
	}
	void OnDisable()
	{
		if ( EventManager.Exists )
		{
			EventManager.StopListening( "End Level", LevelComplete );
		}
	}
	public void Menu()
	{
		menu.SetActive( !menu.activeSelf );
	}
	public void NextLevelButton()
	{
		Controller controller = GameObject.Find("Controller").GetComponent<Controller>();
		if ( controller != null )
		{
			string nextLevel = controller.nextLevel;
			if ( nextLevel == null || nextLevel == "" )
				return;
			else
				UnityEngine.SceneManagement.SceneManager.LoadScene(nextLevel);
		}
	}
	public void Exit()
	{
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Exit();
		#endif
	}
	public void StartMenu()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}
	void LevelComplete()
	{
		Debug.Log( "Level over");
		NextLevelButton();
	}
}