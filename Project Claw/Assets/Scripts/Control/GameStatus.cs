using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStatus : MonoBehaviour {
	public static GameStatus instance;

	public int points = 0;
    public int maxPoints = 0;
	public int checkPoint = 1;
	public string level = "";

	void Awake()
	{
		if ( instance == null )
		{
			instance = this;
		}
		else{
			Destroy(this.gameObject);
		}
		DontDestroyOnLoad(this.gameObject);
	}
	public void Save()
	{
		Debug.Log( "Saving");
		PlayerPrefs.SetInt( "Points", points );
		PlayerPrefs.SetInt( "Check Point", checkPoint );
		if ( level != "" ) PlayerPrefs.SetString( "Level", level );
	}
	public void Load()
	{
		Debug.Log( "Loading" );
		points = PlayerPrefs.GetInt( "Points" );
		checkPoint = PlayerPrefs.GetInt( "Check Point");

		level = PlayerPrefs.GetString( "Level" );
		if ( level != "" )
		{
			SceneManager.LoadScene( level );
		}
	}
	public void NextLevel()
	{
		checkPoint = 1;
//		level ++;
	}
}