using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour {
	static EventManager instance;
	public static bool exists = false;
	public static Dictionary<string, UnityEvent> eventDictionary;

	void Awake()
	{
		if ( instance == null )
		{
			instance = this;
			exists = true;
		}
		else
		{
			Destroy( this );
		}
		if ( eventDictionary == null )
		{
			Debug.Log("Dictionary initialised");
			eventDictionary = new Dictionary<string, UnityEvent>();
		}
	}
	public static void StartListening( string eventName, UnityAction listener )
	{
		UnityEvent thisEvent = null;
		if ( eventDictionary.TryGetValue( eventName, out thisEvent ))
		{
			thisEvent.AddListener( listener );
		}
		else
		{
			thisEvent = new UnityEvent();
			thisEvent.AddListener( listener );
			eventDictionary.Add( eventName, thisEvent );
		}
	}
	public static void StopListening( string eventName, UnityAction listener )
	{
		if ( eventDictionary == null ) return;
		UnityEvent thisEvent;
		if ( eventDictionary.TryGetValue( eventName, out thisEvent))
		{
			thisEvent.RemoveListener( listener);
		}
	}
	public static void TriggerEvent( string eventName )
	{
		UnityEvent thisEvent = null;
		if ( eventDictionary.TryGetValue( eventName, out thisEvent))
		{
			thisEvent.Invoke();
		}
	}
}