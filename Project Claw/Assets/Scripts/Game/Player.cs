using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	[SerializeField]int hp;
	[SerializeField]int points;

	public int HitPoints
	{
		get{
			return hp;
		}
		set{
			hp = value;
		}
	}
	void OnTriggerEnter( Collider collision )
	{
		if ( collision.gameObject.tag == "Pit" )
		{
			EventManager.TriggerEvent( "Pit" );
		}
		if ( collision.gameObject.tag == "Check Point" )
		{
			EventManager.TriggerEvent( "Check Point" );
        }
        if (collision.gameObject.tag == "Collectable")
        {
            //EventManager.TriggerEvent("coinpickup");
            //Debug.Log("Coin, boy");
        }
    }
    void RestartLevel()
	{
//		transform.position = Controller.instance.checkPoints[Controller.instance.currentCheckPoint].transform.position;
//		transform.rotation = Controller.instance.checkPoints[Controller.instance.currentCheckPoint].transform.rotation;
		Destroy( this.gameObject );
		Debug.Log("Hello");
	}
}