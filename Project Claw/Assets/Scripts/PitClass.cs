using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitClass : MonoBehaviour {
	Collider pit;

	void OnCollisionEnter( Collision collision )
	{
		if ( collision.gameObject.tag == "Player" )
		{
			
		}
	}
}