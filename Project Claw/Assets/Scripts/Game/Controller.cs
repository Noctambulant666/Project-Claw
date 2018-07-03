using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour {
	[HideInInspector] public static Controller instance;
	[SerializeField] public GameObject[] checkPoints;
	[SerializeField] GameObject playerPrefab;
	[SerializeField] public string nextLevel = "";
	public int currentCheckPoint = 0;

	void Awake()
	{
		if ( instance == null )
		{
			instance = this;
		}
		else
		{
			Destroy( this.gameObject);
		}
	}
	void OnEnable()
	{
		if ( EventManager.exists )
		{
			EventManager.StartListening( "Pit", Restart );
			EventManager.StartListening( "Check Point", LevelComplete );
		}
	}
	void Start()
	{
		if ( playerPrefab != null )
			Instantiate( playerPrefab, checkPoints[0].transform.position, checkPoints[0].transform.rotation );
		GameStatus.instance.level = SceneManager.GetActiveScene().name;
	}
	void Update()
	{
		if ( GameStatus.instance == null ) return;

		if ( Input.GetKeyDown( KeyCode.F1) )
		{
			GameStatus.instance.Save();
		}
		if ( Input.GetKeyDown( KeyCode.F2) )
		{
			GameStatus.instance.Load();
		}
	}
	public void SwitchLevel()
	{
		if ( nextLevel != "" )
		{
			SceneManager.LoadScene(nextLevel);
		}
	}
	void Restart()
	{
		if ( playerPrefab != null )
		{
			Destroy( GameObject.FindWithTag("Player") );
			Instantiate( playerPrefab, checkPoints[0].transform.position, checkPoints[0].transform.rotation );
		}
	}
	void LevelComplete()
	{
		EventManager.TriggerEvent( "End Level" );
	}
}