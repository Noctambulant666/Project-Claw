using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour {
	[HideInInspector] public static Controller instance; // Following the Unity singleton pattern

    [Header("Object references")]
    [SerializeField] private GameObject[] checkPoints;
	[SerializeField] private GameObject playerPrefab;
	[Tooltip("Enter the scene name of the next level as a string. If left blank, game over screen appears.")]
    public string nextLevel = "";

    [Header("Points")]
    [SerializeField] private int maxPoints = 0;
    [SerializeField] private int points = 0;
    private GameObject player;
    [HideInInspector] public int currentCheckPoint = 0;

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
            EventManager.StartListening("coinpickup", Coin );
        }
	}
	void OnDisable()
	{
		if ( EventManager.Exists )
		{
			EventManager.StopListening( "Pit", Restart );
			EventManager.StopListening( "Restart", Restart );
			EventManager.StopListening( "Check Point", LevelComplete );
            EventManager.StopListening("coinpickup", Coin);
        }
	}
	void Start()
	{
		if ( playerPrefab != null )
        {
			player = Instantiate( playerPrefab, checkPoints[0].transform.position, checkPoints[0].transform.rotation );
        }
		GameStatus.instance.level = SceneManager.GetActiveScene().name;

		if ( EventManager.Exists) 
		{
			// Intro sequence
			StartCoroutine( Intro() );
		}

        foreach ( GameObject g in GameObject.FindGameObjectsWithTag("Collectable") )
        {
            maxPoints += 1;
        }
	}
	IEnumerator Intro()
	{
		// Intro sequence
		EventManager.TriggerEvent("Fade in");
		yield return new WaitForSeconds( 0.7f );
		yield return StartCoroutine( LerpCameraToEndAndBack() );
		EventManager.TriggerEvent( "Has Control");
		if ( AudioManager.instance != null ) AudioManager.instance.Play( "Theme", true );
	}
	IEnumerator QuickIntro()
	{
		// Intro sequence
		EventManager.TriggerEvent("Fade in");
		yield return new WaitForSeconds( 0.7f );
		EventManager.TriggerEvent( "Has Control");
		if ( AudioManager.instance != null ) AudioManager.instance.Play( "Theme", true );
	}
	IEnumerator LerpCameraToEndAndBack()
	{
		GameObject cam;
		cam = Camera.main.gameObject;
		Vector3 camStart = cam.transform.position;
		Vector3 camEnd = new Vector3( checkPoints[1].transform.position.x, cam.transform.position.y, checkPoints[1].transform.position.z+1);
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
            //Destroy( GameObject.FindWithTag("Player") );
            //Instantiate( playerPrefab, checkPoints[0].transform.position, checkPoints[0].transform.rotation );
            EventManager.TriggerEvent("Has Control");
            player.transform.position = checkPoints[0].transform.position;
            player.transform.rotation = checkPoints[0].transform.rotation;
            
			StartCoroutine( QuickIntro() );
			if ( AudioManager.instance != null ) AudioManager.instance.Play( "Pit" );
		}
	}
	void LevelComplete()
	{
		EventManager.TriggerEvent( "Has Control" );

        GameStatus.instance.points = points;
        GameStatus.instance.maxPoints = maxPoints;

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
    void Coin()
    {
        points++;
    }
}