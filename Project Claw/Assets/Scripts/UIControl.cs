using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControl : MonoBehaviour {
	public GameObject menu;
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
}