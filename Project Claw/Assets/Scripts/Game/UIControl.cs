using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour {
	public GameObject menu;
	public GameObject levelOverPanel;
	public GameObject winGamePanel;
	public GameObject optionsPanel;
    public Text levelOverPointsText;
    public Text winPointsText;
    public Text LevelNameText;

    void OnEnable()
	{
		if ( GameObject.Find("Event Manager") != null )
		{
			EventManager.StartListening( "End Level", LevelComplete );
			EventManager.StartListening( "Win Game", WinGame );
		}
        LevelNameText.text = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
	}
	void OnDisable()
	{
		if ( EventManager.Exists )
		{
			EventManager.StopListening( "End Level", LevelComplete );
			EventManager.StartListening( "Win Game", WinGame );
		}
	}
	public void Menu()
	{
		menu.SetActive( !menu.activeSelf );
		optionsPanel.SetActive( false );

        GameObject levName = LevelNameText.gameObject;
        levName.SetActive(!levName.activeSelf);
	}
	public void Options()
	{
		optionsPanel.SetActive( !optionsPanel.activeSelf );
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
		if ( levelOverPanel == null )
		{
			Debug.LogError( "UI Error: Level Over Panel is missing reference");
			return;
		}
		levelOverPanel.SetActive(true);
        levelOverPointsText.text = "" + GameStatus.instance.points + " / " + GameStatus.instance.maxPoints;
    }
	void WinGame()
	{
		Debug.Log( "Game over");
		if ( winGamePanel == null ){
			Debug.LogError( "UI Error: Win Game Panel is missing reference");
			return;
		}
		winGamePanel.SetActive(true);
        winPointsText.text = "" + GameStatus.instance.points + " / " + GameStatus.instance.maxPoints;
    }
	public void Retry()
	{
		levelOverPanel.SetActive(false);
		winGamePanel.SetActive(false);
		EventManager.TriggerEvent("Restart");
	}
}