using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	[SerializeField]int hp;
	[SerializeField]int points;
	[SerializeField]float jump;
	[SerializeField]float speed = 5f;
	Rigidbody rb;


	void Start(){
		rb = transform.GetComponent<Rigidbody>();
	}

	public int HitPoints
	{
		get{
			return hp;
		}
		set{
			hp = value;
		}
	}
	void OnCollisionEnter( Collision collision )
	{
		if ( collision.gameObject.tag == "Pit" || collision.gameObject.tag == "Spikes" )
		{
			RestartLevel();
		}
		if ( collision.gameObject.tag == "Exit" )
		{
			Controller.instance.SwitchLevel();
		}
	}
	void RestartLevel()
	{
		transform.position = Controller.instance.checkPoints[Controller.instance.currentCheckPoint].transform.position;
	}
	void FixedUpdate()
	{
//		float moveHorizontal = Input.GetAxis("Horizontal");
//		float moveVertical = Input.GetAxis("Vertical");
//		Vector3 move = new Vector3( moveHorizontal*speed, 0, moveVertical*speed);
//
//		rb.velocity = move;
	}
}
