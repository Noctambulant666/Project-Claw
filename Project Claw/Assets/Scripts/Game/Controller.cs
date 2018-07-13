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

//	public GameObject cam;
//	public GameObject cam2;

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
		if ( EventManager.Exists )
		{
			EventManager.StartListening( "Pit", Restart );
			EventManager.StartListening( "Restart", Restart );
			EventManager.StartListening( "Check Point", LevelComplete );
		}
	}
	void OnDisable()
	{
		if ( EventManager.Exists )
		{
			EventManager.StopListening( "Pit", Restart );
			EventManager.StopListening( "Restart", Restart );
			EventManager.StopListening( "Check Point", LevelComplete );
		}
	}
	void Start()
	{
		if ( playerPrefab != null )
			Instantiate( playerPrefab, checkPoints[0].transform.position, checkPoints[0].transform.rotation );
		GameStatus.instance.level = SceneManager.GetActiveScene().name;

		if ( EventManager.Exists) 
		{
			// Intro sequence
			StartCoroutine( Intro() );
		}
	}
	IEnumerator Intro()
	{
		// Intro sequence
		EventManager.TriggerEvent("Fade in");
		yield return new WaitForSeconds( 0.7f );
		yield return StartCoroutine( LerpToEndAndBack() );
		EventManager.TriggerEvent( "Has Control");
		if ( AudioManager.instance != null ) AudioManager.instance.Play( "Theme", true );
	}
	IEnumerator QuickIntro()
	{
		// Intro sequence
		EventManager.TriggerEvent("Fade in");
		yield return new WaitForSeconds( 0.7f );
//		yield return StartCoroutine( LerpToEndAndBack() );
		EventManager.TriggerEvent( "Has Control");
		if ( AudioManager.instance != null ) AudioManager.instance.Play( "Theme", true );
	}
	IEnumerator LerpToEndAndBack()
	{
		GameObject cam;
		cam = Camera.main.gameObject;
		Vector3 camStart = cam.transform.position;
//		Vector3 camEnd = cam2.transform.position;
		Vector3 camEnd = new Vector3( checkPoints[1].transform.position.x, cam.transform.position.y, checkPoints[1].transform.position.z+1);
//		Vector3 camPos = camStart;
		float speed = 10f;
		while ( cam.transform.position != camEnd )
		{
			cam.transform.position = Vector3.Lerp( cam.transform.position, camEnd, Time.deltaTime * speed );
			yield return null;
		}
		yield return new WaitForSeconds(0.5f);
		while ( cam.transform.position != camStart )
		{
			cam.transform.position = Vector3.Lerp( cam.transform.position, camStart, Time.deltaTime * speed );
			yield return null;
		}
		yield return null;
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
		else
		{
			WinGame();
		}
	}
	void Restart()
	{
		if ( playerPrefab != null )
		{
			Destroy( GameObject.FindWithTag("Player") );
			Instantiate( playerPrefab, checkPoints[0].transform.position, checkPoints[0].transform.rotation );
//			EventManager.TriggerEvent("Has Control");
			StartCoroutine( QuickIntro() );
			if ( AudioManager.instance != null ) AudioManager.instance.Play( "Pit" );
		}
	}
	void LevelComplete()
	{
		EventManager.TriggerEvent( "Has Control" );
		if ( nextLevel == "" || nextLevel == null )
		{
			Debug.Log( "Win?");
			WinGame();
		}
		else
		{
//			EventManager.TriggerEvent( "Has Control" );
			EventManager.TriggerEvent( "End Level" );
			if ( AudioManager.instance != null ) AudioManager.instance.Play( "End Level" );
		}
	}
	void WinGame()
	{
		if ( AudioManager.instance != null )
		{
			AudioManager.instance.Pause( "Theme" );
			AudioManager.instance.Play( "Win game" );
		}
		if ( EventManager.Exists ) EventManager.TriggerEvent( "Win Game" );
	}
}